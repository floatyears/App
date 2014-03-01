##install go environment:

1. download go src: http://go.googlecode.com/files/go1.2.linux-amd64.tar.gz
2. tar zxvf go1.2.linux-amd64.tar.gz -C /usr/local
3. cd /usr/loca/go/src;./all.bash
4. add .bash_profile:
export GOROOT=/usr/local/go
export GOPATH=/usr/local/bbsvr/go/3rdparty
export PATH=$PATH:$GOROOT/bin
5. done.
