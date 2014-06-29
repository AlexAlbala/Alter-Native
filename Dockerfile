# A basic AlterNative server. To use either add or bind mount content under /var/www
FROM ubuntu:14.04

MAINTAINER Gerard Solé: 0.1

RUN apt-get update && apt-get install -y mono-complete cmake libboost-all-dev git
RUN cd /
RUN git clone https://github.com/AlexAlbala/Alter-Native.git
ENV BOOST_INCLUDEDIR /usr/include
WORKDIR /Alter-Native
CMD git pull && git submodule init && git submodule update && source ./install.sh
