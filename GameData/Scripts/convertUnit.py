#!/usr/bin/python
#coding=utf-8
### created by kory on 09/22/2013 ###
import sys
import re
import urllib2

MaxUnitNum=646

basePath='/Users/kory/Documents/Dev/server/bbmsvr/bbsvr/test/unitData'
INDEX_FILE=basePath+"/unit.html"
SAVE_FILE=basePath+'/newunit.html'

#write to target file
fileSave = open(SAVE_FILE, "w+")

def trimNumber(strNum):
    i=0
    while i<len(strNum):
        if strNum[i]!="0":
            return strNum[i:]
        i+=1
    return strNum

# print "trimNumber='%s'" % trimNumber("0029")

def trimName(name):
    name=re.sub('&#160;','',name)
    name=re.sub('<span style="display:inline-block;">','', name)
    return name

def substr(tag1,tag2, text, pos):
    # tag=tag1+"(.*)"+tag2
    newpos=pos
    p1 = text.find(tag1, pos)
    # print "p1:", tag1
    if p1 >= 0:
        p1=p1+len(tag1)
        newpos=p1
        body=text[p1:]
        # print "body=%d %s" % (len(body),body)
        p2=body.find(tag2)
        if p2 >= 0:
            body=body[:p2]
            newpos+=p2
        return body.strip(),newpos
    return "", newpos

def parseNormalSkillColors( str ):
    colors=str.split("<img alt=")
    normalSkillColors=""
    # print 'colors count=', len(colors)
    for c in colors:
        if c.find('" src="http') > 0 :
            c, tmp=substr('"', '"', c, 0)
            normalSkillColors=normalSkillColors+c+" "

            # else:
            #     print 'unknown c:', c
    normalSkillColors=normalSkillColors.strip()
    return normalSkillColors


def ParseUnitPage():
    ff = open(INDEX_FILE,'r')
    html=ff.read()
    ff.close

    count=0
    f = open(INDEX_FILE,'r')
    while True:
        line = f.readline()
        if line=='':
            break

        print "line:"+line
        pos=0
        id, pos=substr('<td>', '<', line, pos)
        if id=="":
            continue
        id=trimNumber(id)

        href, pos=substr('href="', '"', line, pos)
        src, pos=substr('src="', '"', line, pos)
        newline=re.sub(href+'" ', '' +id+ '.png" target=_blank ', line)
        newline=re.sub(src, ""+id+".png", newline)

        # print "href:"+href
        print "newline:"+newline
        html=html.replace(line, newline)
        # print "new:"+new
        # if int(id)>50:
        #     break



    # print 'unitInfo:', unitInfo
    f.close
    fileSave.write(html)

ParseUnitPage()



fileSave.close