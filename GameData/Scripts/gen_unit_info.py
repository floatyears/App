#!/usr/bin/python
#coding=utf-8
### created by kory on 09/22/2013 ###
import sys
import re
import urllib2

MaxUnitNum=226

#basePath='/Users/kory/Documents/Dev/server/bbmsvr/bbsvr/test/unitData'
basePath='./'
INDEX_FILE=basePath+"/index%d.html" % (MaxUnitNum)
SAVE_FILE=basePath+'/UnitInfo%d.csv' % (MaxUnitNum)

#write to target file
fileSave = open(SAVE_FILE, "w+")
fileSave.write('"id","name","series","type","race","cost","star","maxLv","maxExp","minHp","maxHp","minAtk","maxAtk","hpGrow","atkGrow","blendValue","saleValue",\
"leaderSkillName","leaderSkillEffect","activeSkillName","activeSkillEffect","activeSkillFactor","initCD","minCD",\
"normalSkillName","normalSkillEffect","normalSkillFactor"," normalSkillColors",\
"normalSkillName2","normalSkillEffect2","normalSkillFactor2"," normalSkillColors2",\
"passiveSkillName","passiveSkillEffect",\
"getPath1","getPath2","getPath3",\
"evolveParts","evoTargetUnitId","evoFriendType","evoFriendRace","evoFriendLv"\n')

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


def ParseUnitPage(html):

    # f = open(FILENAME,'r')
    # html = f.read()
    # f.close

    tableTag1='<table class="wikitable" style="text-align: center">'
    tableTag2='</table>'

    pos=0
    table, pos = substr(tableTag1, tableTag2, html, pos)


    tr,pos=substr("<tr>","</tr", table, 0)
    # print "tr: pos=%d tr='%s'" % (pos, tr)


    id, pt=substr('a>', '<', tr, 0)
    # id=substr(">", "<", "a> xb </a")

    td, pt= substr('<td', '/td', tr, pt)
    name, tmp= substr('small>(', ')<', td, 0)
    if name=='':
        name,tmp= substr('>', '<', td, 0)
    name=trimName(name)

    type,pt= substr('<td colspan="3" style="width: 75px;">', '<', tr, pt)
    cost,pt= substr('<td colspan="3">', '<', tr, pt)

    #tr2
    tr,pos=substr("<tr>","</tr", table, pos)
    # print "tr2:+ pos=%d tr='%s'" % (pos, tr)
    pt=0
    race, pt=substr('<td colspan="3" style="width: 75px;">', '<', tr, pt)
    maxLv, pt=substr('id="chara-lvmax">', '<', tr, pt)
    print "pos=%d race='%s' maxlv='%s' type='%s' cost='%s' " % (pos, race, maxLv, type,cost)

    #tr3
    tr,pos=substr("<tr>","</tr", table, pos)
    # print "tr: pos=%d tr='%s'" % (pos, tr)
    pt=0

    series, pt=substr('<a ', '/a>', tr, pt)
    series, tmp=substr('>', '<', series, 0)
    star, pt=substr('style="width: 75px;">★', '</td', tr, pt)
    maxExp, pt=substr('id="chara-expmax" style="width: 75px;">', '<', tr, pt)
    print "pos=%d star='%s' maxExp='%s' type='%s' cost='%s' " % (pos, star, maxExp, type,cost)


    #tr4 基本屬性 HP
    tr,pos=substr("<tr>","</tr", table, pos)
    # print "tr: pos=%d tr='%s'" % (pos, tr)
    pt=0
    minHp, pt=substr('id="chara-hpmin" style="width: 75px;">', '</td', tr, pt)
    maxHp, pt=substr('id="chara-hpmax" style="width: 75px;">', '</td', tr, pt)
    hpGrow, pt=substr('<span ', '/span', tr, pt)
    hpGrow, tmp=substr('>', '<', hpGrow, 0)
    blendValue, pt=substr(' id="chara-blendmin">', '</td', tr, pt)
    print "pos=%d minHp='%s' maxHp='%s' hpGrow='%s' blendValue='%s' " % (pos, minHp, maxHp, hpGrow,blendValue)

    #tr5 ATK
    tr,pos=substr("<tr>","</tr", table, pos)
    pt=0
    minAtk, pt=substr('id="chara-atkmin">', '</td', tr, pt)
    maxAtk, pt=substr('id="chara-atkmax">', '</td', tr, pt)
    atkGrow, pt=substr('<span ', '/span', tr, pt)
    atkGrow, tmp=substr('>', '<', atkGrow, 0)
    saleValue, pt=substr('id="chara-salesmin">', '</td', tr, pt)
    print "pos=%d minAtk='%s' maxAtk='%s' atkGrow='%s' saleValue='%s' " % (pos, minAtk, maxAtk, atkGrow,saleValue)


    #tr6 队长技能名称
    tr,pos=substr("<tr>","</tr", table, pos)
    pt=0
    td, pt=substr('<td', '</td>', tr, pt)
    if td.find("<small>(") > 0 :
        leaderSkillName, tmp=substr('<small>(', ')', td, 0)
    else:
        leaderSkillName, tmp=substr('title="LS:', '"', td, 0)
    leaderSkillName=trimName(leaderSkillName)
    #tr7 队长技能效果
    tr,pos=substr("<tr>","</tr", table, pos)
    pt=0
    leaderSkillEffect, pt=substr('〔', '〕', tr, pt)
    leaderSkillEffect=re.sub('<span class="chara-atk-dep" data-atk-factor="1.5"></span>','',leaderSkillEffect)

    print "pos=%d leaderSkillName='%s' leaderSkillEffect='%s'" % (pos, leaderSkillName, leaderSkillEffect)

    #tr8 主动技能名称
    tr,pos=substr("<tr>","</tr", table, pos)
    pt=0
    td, pt=substr('<td', '</td>', tr, pt)
    if td.find("<small>(") > 0 :
        activeSkillName, tmp=substr('<small>(', ')', td, 0)
    else:
        activeSkillName, tmp=substr('title="AS:', '"', td, 0)
    activeSkillName=trimName(activeSkillName)

    tmp, pt=substr('style="width: 50px;">', '</t', tr, pt)
    initCD, pt=substr('style="width: 50px;">', '</t', tr, pt)
    tmp, pt=substr('style="width: 50px;">', '</t', tr, pt)
    minCD, pt=substr('style="width: 50px;">', '</t', tr, pt)
    #tr9 主动技能效果
    tr,pos=substr("<tr>","</tr", table, pos)
    pt=0
    activeSkillEffect, pt=substr('〔', '<', tr, pt)
    activeSkillEffect=re.sub('〕','',activeSkillEffect).strip()
    activeSkillEffect=re.sub('（','',activeSkillEffect).strip()
    activeSkillFactor, pt=substr('data-atk-factor="', '"', tr, pt)
    # activeSkillEffect=re.sub('<span class="chara-atk-dep" data-atk-factor="3"></span>','',activeSkillEffect)
    print "pos=%d activeSkillName='%s' activeSkillEffect='%s' activeSkillFactor='%s'" % (pos, activeSkillName, activeSkillEffect,activeSkillFactor)
    print "pos=%d initCD='%s' minCD='%s'" % (pos, initCD, minCD)
    if initCD=='':
        initCD="0"
    if minCD=='':
        minCD="0"

    #tr10 普通技能名称
    tr,pos=substr("<tr>","</tr", table, pos)
    pt=0

    normalSkillName=''
    td, pt=substr('<td', '</td>', tr, pt)
    if td.find("<small>(") > 0 :
        normalSkillName, tmp=substr('<small>(', ')', td, 0)
    else:
        normalSkillName, tmp=substr('title="NS:', '"', td, 0)
    normalSkillName=trimName(normalSkillName)

    td,pt=substr('<td colspan="6" style="width: 150px;">','</td', tr, pt)
    # colors=re.findall("<img alt=.*", td, re.DOTALL)
    normalSkillColors=parseNormalSkillColors( td )


    #tr11 普通技能效果
    tr,pos=substr("<tr>","</tr", table, pos)
    pt=0
    normalSkillEffect, pt=substr('〔', '<', tr, pt)
    normalSkillEffect=re.sub('〕','',normalSkillEffect).strip()

    normalSkillFactor, pt=substr('data-atk-factor="', '"', tr, pt)
    print "pos=%d normalSkillName='%s' normalSkillEffect='%s' normalSkillFactor='%s' normalSkillColors=‘%s’" % (pos, normalSkillName, normalSkillEffect,normalSkillFactor, normalSkillColors)

    #normalskill2
    normalSkillName2=normalSkillEffect2=normalSkillFactor2=normalSkillColors2=''
    passiveSkillName=passiveSkillEffect=''
    tr,pos=substr("<tr>","</tr", table, pos)
    if tr.find('技能') > 0:
        isPassive=False
        if tr.find('被动') > 0 :
            isPassive=True
        pt=0
        td, pt=substr('<td', '</td>', tr, pt)
        print("td:"+td)
        if td.find("<small>(") > 0 :
            normalSkillName2, pt=substr('<small>(', ')', td, 0)
        elif isPassive==True:
            normalSkillName2, pt=substr('title="PS:', '"', td, 0)
        else:
            normalSkillName2, pt=substr('title="NS:', '"', td, 0)

        normalSkillName2=trimName(normalSkillName2)

        td,pt=substr('<td colspan="6" style="width: 150px;">','</td', tr, pt)
        # colors=re.findall("<img alt=.*", td, re.DOTALL)
        normalSkillColors2=parseNormalSkillColors( td )

        #tr11 普通技能2效果
        tr,pos=substr("<tr>","</tr", table, pos)
        pt=0
        normalSkillEffect2, pt=substr('〔', '<', tr, pt)
        normalSkillEffect2=re.sub('〕','',normalSkillEffect2).strip()
        normalSkillFactor2, pt=substr('data-atk-factor="', '"', tr, pt)

        if isPassive==True :
            passiveSkillName=normalSkillName2
            passiveSkillEffect=normalSkillEffect2
            normalSkillName2=normalSkillEffect2=normalSkillFactor2=normalSkillColors2=''
            print "pos=%d passiveSkillName='%s' passiveSkillEffect='%s'" % (pos, passiveSkillName, passiveSkillEffect)

        print "pos=%d normalSkillName2='%s' normalSkillEffect2='%s' normalSkillFactor2='%s' normalSkillColors2=‘%s’" % (pos, normalSkillName2, normalSkillEffect2,normalSkillFactor2, normalSkillColors2)
        tr,pos=substr("<tr>","</tr", table, pos)
    # else:
    #     print("tr not found技能:"+tr)

    getPath1=getPath2=getPath3=''
    if tr.find('取得方法') > 0:
        pt=0
        td, pt=substr('<td ', '/td', tr, pt)
        getPath1, tmp=substr('>', '<', td, 0)
        td, pt=substr('<td ', '/td', tr, pt)
        getPath2, tmp=substr('>', '<', td, 0)
        # getPath2, pt=substr('class="table-no">', '</t', tr, pt)
        if getPath2=='':
            getPath2, pt=substr('"partial table-partial">', '</t', tr, pt)
        getPath2=re.sub('"','',getPath2)
        getPath2=re.sub('<br />','',getPath2).strip()
        getPath2=re.sub('<br>','',getPath2).strip()
        getPath3, pt=substr('colspan="10">', '</t', tr, pt)
        print 'getPath1:%s getPath2:%s getPath3:%s ' % (getPath1,getPath2,getPath3)

    #出现的副本
    tr,pos=substr("<tr>","</tr", table, pos)

    #进化材料
    evolveParts=''
    evoTargetUnitId="0"
    evoFriendType='不限'
    evoFriendRace='不限'
    evoFriendLv='0'
    tr,pos=substr("<tr>","</tr", table, pos)
    pt=0
    evolveData, pt=substr('data-image-key="Plus.png"', '<img alt="Evoto.png"', tr, pt)
    if len(evolveData)>0:
        # print "tr:", tr
        # print "evolveData:", evolveData
        partlist=evolveData.split('<a ')
        for part in partlist:
            if part.find('title="ID:')>0:
                partId, tmp=substr('title="ID:', ' ', part, 0)
                evolveParts=evolveParts+trimNumber(partId)+" "
                print "partId:", partId

        evolveParts=evolveParts.strip()
        print "evolveParts:'%s'" % evolveParts

        href, pt=substr('<a ', '</a', tr, pt)
        if href.find('title="ID:')>0:
            evoTargetUnitId, tmp=substr('title="ID:', ' ', href, 0)

        tr, pos=substr("<tr>","</tr", table, pos)
        if tr.find("朋友")>0:
            tr, pos=substr("<tr>","</tr", table, pos)
            tmp=0
            td1, tmp=substr("<td","/td", tr, tmp)
            td2, tmp=substr("<td","/td", tr, tmp)
            td3, tmp=substr("<td","/td", tr, tmp)
            evoFriendRace, tmp=substr(">","<", td1, 0)
            evoFriendLv, tmp=substr(">","<", td2, 0)
            evoFriendType, tmp=substr(">","<", td3, 0)
            print "evoFriend: %s %s %s" % (evoFriendType, evoFriendRace, evoFriendLv)

    unitInfo='"%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s"'\
             % (id,name,series,type,race,cost,star,maxLv,maxExp,minHp,maxHp,minAtk,maxAtk,hpGrow,atkGrow,blendValue,saleValue,\
            leaderSkillName,leaderSkillEffect,activeSkillName,activeSkillEffect,activeSkillFactor,initCD,minCD, \
            normalSkillName,normalSkillEffect,normalSkillFactor, normalSkillColors, \
            normalSkillName2,normalSkillEffect2,normalSkillFactor2, normalSkillColors2, \
            passiveSkillName,passiveSkillEffect,
            getPath1,getPath2,getPath3,
            evolveParts,evoTargetUnitId,evoFriendType,evoFriendRace,evoFriendLv)
    fileSave.write(unitInfo+"\n")
    print 'unitInfo:', unitInfo

def ParseIndexPage():
    f = open(INDEX_FILE,'r')
    html = f.read()
    f.close
    pt=0
    count=0
    while count<MaxUnitNum:
        link, pt=substr('<td', '</td>', html, pt)
        link, tmp=substr('<a ', '</a>', link, 0)
        link, tmp=substr('href="', '"', link, 0)
        link='http://zh.divine-gate.wikia.com'+link+'?variant=zh-hans'
        count+=1

        print "link[%d]: %s" % (count, link)

        # targetID=3
        # if count<targetID:
        #     continue
        # if count>targetID:
        #     break


        request=urllib2.Request(link)
        html_str=urllib2.urlopen(request).read()
        # print 'html_str:', html_str
        ParseUnitPage(html_str)


        # if count > 1:
        #     break


ParseIndexPage()



fileSave.close