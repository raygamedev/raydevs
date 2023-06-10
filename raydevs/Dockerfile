FROM unityci/editor:2022.3.0f1-webgl-1

WORKDIR /app

COPY . .

RUN unity -batchmode -quit -projectPath . -buildTarget WebGL

CMD ["/bin/bash"]