# A basic AlterNative server.
FROM ubuntu:14.04

MAINTAINER Gerard Solé: 0.1

RUN apt-get update && apt-get install -y mono-complete cmake libboost-all-dev git
RUN cd /
RUN git clone https://github.com/AlexAlbala/Alter-Native.git
ENV BOOST_INCLUDEDIR /usr/include
WORKDIR /Alter-Native
CMD echo "Hello To AlterNative container!" && git pull && git submodule init && git submodule update && source ./install.sh
