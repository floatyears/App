currPath=`dirname $0`
cd $currPath
language=$1
rootpath=../../
destFont="${rootpath}/Assets/Resources/Font/Dimbo Regular.ttf"
if [ "${language}" = "cn" ]; then
	cp -vf $rootpath"Doc/Font/文泉驿微米黑.ttf" "$destFont"
	echo cp 文泉驿微米黑.ttf
else
	echo cp Dimbo.ttf
	cp -vf $rootpath"Doc/Font/Dimbo Regular.ttf" "$destFont"
fi
