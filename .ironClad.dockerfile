FROM debian:latest

RUN if getent passwd 1000; then userdel -f $(getent passwd 1000 | cut -d ":" -f 1); fi
RUN if getent group 100; then groupdel -f $(getent group 100 | cut -d ":" -f 1); fi

RUN groupadd -g 100 clad
RUN useradd -m -s /bin/bash -g 100 -u 1000 clad