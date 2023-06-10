# Build the React app
FROM node:18 AS react-build
WORKDIR /app
COPY client/package*.json ./
RUN npm install
COPY client/ ./
RUN npm run build

# Build the Rust server
FROM rust:1.69 AS rust-build
WORKDIR /app
COPY server/Cargo.toml server/Cargo.lock ./
# Create a dummy main.rs to build the dependencies and cache them
RUN mkdir -p src && \
    echo 'fn main() {}' > src/main.rs && \
    cargo build --release && \
    rm -rf src
COPY server/ ./
RUN cargo install --path .

# Combine the React app and Rust server into the final image
FROM debian:buster-slim
RUN apt-get update && apt-get install -y libssl-dev ca-certificates && rm -rf /var/lib/apt/lists/*
COPY --from=react-build /app/build /app/build
COPY --from=rust-build /usr/local/cargo/bin/server /app/server
COPY server/assets /app/assets 

WORKDIR /app
CMD ["/app/server"]