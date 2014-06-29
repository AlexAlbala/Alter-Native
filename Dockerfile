# A basic AlterNative server. To use either add or bind mount content under /var/www
FROM ubuntu:14.04

MAINTAINER Gerard Solé: 0.1

RUN apt-get update && apt-get install -y mono cmake libboost-all-dev git && apt-get clean && rm -rf /var/lib/apt/lists/*
RUN git clone https://github.com/AlexAlbala/Alter-Native.git
WORKDIR ~/Alter-Native
CMD git pull
CMD git submodule init
CMD git submodule update
CMD source ./install.sh
