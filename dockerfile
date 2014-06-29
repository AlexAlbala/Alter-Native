############################################################
# Dockerfile to build AlterNative container images
# Based on Ubuntu 14.04
############################################################
FROM ubuntu:14.04
MAINTAINER Gerard Solé: 0.1

RUN apt-get update && apt-get install -y mono-complete cmake libboost-all-dev git build-essential
RUN cd /
RUN git clone https://github.com/AlexAlbala/Alter-Native.git
ENV BOOST_INCLUDEDIR /usr/include
WORKDIR /Alter-Native
RUN cd /Alter-Native
RUN git pull && git submodule init Lib && git submodule update
ADD https://raw.githubusercontent.com/AlexAlbala/Alter-Native/master/DockerInitScript.sh /Alter-Native/
ADD https://raw.githubusercontent.com/AlexAlbala/Alter-Native/master/DockerAlternative.sh /Alter-Native/
RUN sh ./DockerInitScript.sh
RUN chmod +x DockerAlternative.sh
VOLUME ["/executables"]
ENTRYPOINT ["./DockerAlternative.sh"]
CMD ["Blank.exe"]
