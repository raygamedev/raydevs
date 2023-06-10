FROM ubuntu:22.04
RUN apt-get update && apt-get install -y \
    ssh \
    python3 \
    python3-pip \
    python3-venv \
    python3-wheel \
    python3-setuptools \
    python3-dev \
    build-essential

RUN pip3 install --upgrade pip \
    pip3 install --upgrade virtualenv; \
    pip3 install pywinrm[kerberos]; \
    pip3 install pywinrm; \
    pip3 install jmspath; \
    pip3 install requests; \
    python3 -m pip install ansible;

COPY . /root