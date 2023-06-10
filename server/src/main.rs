use actix_files::Files;
use actix_web::{App, HttpServer};
use actix_web::{web, HttpResponse, Result};
use actix_cors::Cors;
use std::fs;

async fn download_file() -> Result<HttpResponse> {
    // Set the file path to be downloaded.
    let file_path = "assets/dan_raymond_resume.pdf";

    // Read the file contents.
    let file_contents = fs::read(file_path)?;

    // Create a response with the file contents and appropriate headers.
    let response = HttpResponse::Ok()
        .header("Content-Disposition", format!("attachment; filename=\"{}\"", "dan_raymond_resume.pdf"))
        .content_type("application/octet-stream")
        .body(file_contents);

    Ok(response)
}

async fn run() -> std::io::Result<()> {
    // Start the server with the provided address.
    let addr = "0.0.0.0:3000";
    println!("Serving on http://{}", addr);

    HttpServer::new(|| {
        let cors = Cors::permissive(); // You can customize the CORS settings if needed

        App::new()
        .wrap(cors)
            // Serve files from the React build directory (e.g., "build" or "dist").
        .route("/download", web::get().to(download_file))
        .service(Files::new("/", "build").index_file("index.html"))
        

    })
    .bind(addr)?
    .run()
    .await
}

#[actix_rt::main]
async fn main() {
    println!("Starting server...");
    if let Err(err) = run().await {
        eprintln!("Error: {}", err);
        std::process::exit(1);
    }
}