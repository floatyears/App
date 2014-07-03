#!/usr/bin/python
#coding=utf-8
### created by kory on 07/01/2014 ###
import sys
import re
import os
import urllib2

MaxUnitNum=226

wikiHost='http://zh.divine-gate.wikia.com'

droprateStageUrls=[]
droprateStageUrls.append("/wiki/%E5%88%9D%E9%83%BD%E3%83%8E%E3%83%BC%E3%83%9E%E3%83%AA%E3%82%A2")
droprateStageUrls.append("/wiki/%E7%81%AB%E9%83%BD%E3%83%95%E3%82%A1%E3%82%A4%E3%82%A2%E3%83%AA%E3%82%A2")
droprateStageUrls.append("/wiki/%E6%B0%B4%E9%83%BD%E3%82%A2%E3%82%AF%E3%82%A2%E3%83%AA%E3%82%A2")
droprateStageUrls.append("/wiki/%E9%A2%A8%E9%83%BD%E3%82%A6%E3%82%A3%E3%83%B3%E3%83%80%E3%83%AA%E3%82%A2")
droprateStageUrls.append("/wiki/%E5%85%89%E9%83%BD%E3%83%A9%E3%82%A4%E3%83%A9%E3%83%AA%E3%82%A2")
droprateStageUrls.append("/wiki/%E9%97%87%E9%83%BD%E3%83%80%E3%82%AF%E3%82%BF%E3%83%AA%E3%82%A2")
droprateStageUrls.append("/wiki/%E7%84%A1%E9%83%BD%E3%82%A4%E3%83%B3%E3%83%95%E3%82%A1%E3%82%BF%E3%83%AA%E3%82%A2")
droprateStageUrls.append("/wiki/%E7%AC%AC%E4%B8%80%E7%9B%A3%E7%8D%84%E3%82%AB%E3%83%BC%E3%83%9E%E3%82%A4%E3%83%B3")
droprateStageUrls.append("/wiki/%E7%AC%AC%E4%BA%8C%E7%9B%A3%E7%8D%84%E3%82%B3%E3%83%90%E3%83%AB%E3%83%88")
droprateStageUrls.append("/wiki/%E7%AC%AC%E4%B8%89%E7%9B%A3%E7%8D%84%E3%83%93%E3%83%AA%E3%82%B8%E3%82%A2%E3%83%B3")
droprateStageUrls.append("/wiki/%E7%AC%AC%E5%9B%9B%E7%9B%A3%E7%8D%84%E3%82%AB%E3%83%8A%E3%83%AA%E3%83%A4")
droprateStageUrls.append("/wiki/%E7%AC%AC%E4%BA%94%E7%9B%A3%E7%8D%84%E3%83%A2%E3%83%BC%E3%83%96")
droprateStageUrls.append("/wiki/%E7%AC%AC%E5%85%AD%E7%9B%A3%E7%8D%84%E3%83%81%E3%83%A4%E3%82%B3%E3%83%BC%E3%83%AB")
droprateStageUrls.append("/wiki/%E7%AC%AC%E4%B8%83%E7%9B%A3%E7%8D%84%E3%82%B9%E3%83%9A%E3%82%AF%E3%83%88%E3%83%AB")
droprateStageUrls.append("/wiki/%E6%9F%98%E6%A6%B4%E5%A1%94%E3%82%AC%E3%83%BC%E3%83%8D%E3%83%84%E3%83%88")
droprateStageUrls.append("/wiki/%E8%97%8D%E7%8E%89%E5%A1%94%E3%82%A2%E3%82%AF%E3%82%A2%E3%83%9E%E3%83%AA%E3%83%B3")
droprateStageUrls.append("/wiki/%E7%BF%A0%E7%8E%89%E5%A1%94%E3%82%A8%E3%83%A1%E3%83%A9%E3%83%AB%E3%83%89")
droprateStageUrls.append("/wiki/%E9%BB%84%E7%8E%89%E5%A1%94%E3%83%88%E3%83%91%E3%83%BC%E3%82%BA")
droprateStageUrls.append("/wiki/%E7%B4%AB%E6%99%B6%E5%A1%94%E3%82%A2%E3%83%A1%E3%82%B8%E3%82%B9%E3%83%88")
droprateStageUrls.append("/wiki/%E6%B0%B4%E6%99%B6%E5%A1%94%E3%82%AF%E3%82%A9%E3%83%BC%E3%83%84")
droprateStageUrls.append("/wiki/%E6%A5%B5%E5%85%89%E5%A1%94%E3%82%AA%E3%83%BC%E3%83%AD%E3%83%A9")
droprateStageUrls.append("/wiki/%E8%B5%A4%E5%B8%9D%E6%A5%BC%E9%96%A3%E3%82%B9%E3%82%B6%E3%82%AF")
droprateStageUrls.append("/wiki/%E9%9D%92%E5%B8%9D%E6%A5%BC%E9%96%A3%E3%82%BB%E3%82%A4%E3%83%AA%E3%83%A5%E3%82%A6")
droprateStageUrls.append("/wiki/%E7%B7%91%E5%B8%9D%E6%A5%BC%E9%96%A3%E3%82%B2%E3%83%B3%E3%83%96")
droprateStageUrls.append("/wiki/%E9%BB%84%E5%B8%9D%E6%A5%BC%E9%96%A3%E3%82%B3%E3%82%A6%E3%83%AA%E3%83%A5%E3%82%A6")
droprateStageUrls.append("/wiki/%E7%B4%AB%E5%B8%9D%E6%A5%BC%E9%96%A3%E3%83%93%E3%83%A3%E3%83%83%E3%82%B3")
droprateStageUrls.append("/wiki/%E7%84%A1%E5%B8%9D%E6%A5%BC%E9%96%A3%E3%82%AD%E3%83%AA%E3%83%B3")
droprateStageUrls.append("/wiki/%E7%99%BD%E5%B8%9D%E6%A5%BC%E9%96%A3%E3%82%B7%E3%83%A5%E3%83%A9")
droprateStageUrls.append("/wiki/%E3%83%A9%E3%82%A6%E3%83%B3%E3%82%B8%EF%BC%9A%E3%83%9E%E3%83%BC%E3%82%BA")
droprateStageUrls.append("/wiki/%E3%83%A9%E3%82%A6%E3%83%B3%E3%82%B8%EF%BC%9A%E3%83%9E%E3%83%BC%E3%82%AD%E3%83%A5%E3%83%AA")
droprateStageUrls.append("/wiki/%E3%83%A9%E3%82%A6%E3%83%B3%E3%82%B8%EF%BC%9A%E3%82%B8%E3%83%A5%E3%83%94%E3%82%BF%E3%83%BC")
droprateStageUrls.append("/wiki/%E3%83%A9%E3%82%A6%E3%83%B3%E3%82%B8%EF%BC%9A%E3%83%93%E3%83%BC%E3%83%8A%E3%82%B9")
droprateStageUrls.append("/wiki/%E3%83%A9%E3%82%A6%E3%83%B3%E3%82%B8%EF%BC%9A%E3%82%B5%E3%82%BF%E3%83%BC%E3%83%B3")
droprateStageUrls.append("/wiki/%E3%83%A9%E3%82%A6%E3%83%B3%E3%82%B8%EF%BC%9A%E3%82%A2%E3%83%BC%E3%82%B9")
droprateStageUrls.append("/wiki/%E3%83%A9%E3%82%A6%E3%83%B3%E3%82%B8%EF%BC%9A%E3%83%A0%E3%83%BC%E3%83%B3")
#研究所系列：缺掉率数据
# droprateStageUrls.append("/wiki/%E7%81%AB%E7%84%94%E7%A0%94%E3%83%95%E3%83%AD%E3%82%AE%E3%82%B9%E3%83%88%E3%83%B3")
# droprateStageUrls.append("/wiki/%E6%B0%B7%E6%B0%B4%E7%A0%94%E3%82%A2%E3%83%A2%E3%83%AB%E3%83%95%E3%82%A9%E3%82%B9")
# droprateStageUrls.append("/wiki/%E6%A5%B5%E9%A2%A8%E7%A0%94%E3%82%B3%E3%83%AA%E3%82%AA%E3%83%AA")
# droprateStageUrls.append("/wiki/%E5%B9%BB%E5%85%89%E7%A0%94%E3%83%9B%E3%83%AD%E3%82%B0%E3%83%A9%E3%83%95")
# droprateStageUrls.append("/wiki/%E6%BC%86%E9%BB%92%E7%A0%94%E3%82%AF%E3%82%A4%E3%83%B3%E3%83%86%E3%82%BB%E3%83%B3%E3%82%B9")
# droprateStageUrls.append("/wiki/%E8%99%9A%E7%84%A1%E7%A0%94%E3%82%AB%E3%83%AB%E3%83%84%E3%82%A1%E3%82%AF%E3%83%A9%E3%82%A4%E3%83%B3")
# droprateStageUrls.append("/wiki/%E9%9B%BB%E7%A3%81%E7%A0%94%E3%83%AD%E3%83%BC%E3%83%AC%E3%83%B3%E3%83%84")

dataPath='/Users/kory/Documents/Dev/BB002/bb002/Data/QuestData'


def trimNumber(strNum):
    strNum=re.sub('&#160;','',strNum).strip()
    strNum=re.sub(',','',strNum).strip()
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
    p1=0
    if tag1!="" :
        p1 = text.find(tag1, pos)

    if p1 >= 0:
        # print "p1>0:", tag1
        p1=p1+len(tag1)
        newpos=p1
        body=text[p1:]
        # print "body=%d %s" % (len(body),body)
        p2=body.find(tag2)
        if p2 >= 0:
            # print "p2>0: %s p2:%d %s" % (tag2, p2, body)
            body=body[:p2]
            newpos+=p2
            # print "p2>0: %s p2:%d %s" % (tag2, p2, body)

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


def parseEnemyTable(nameId, enemyType, table, dropRateMap):
    enemyId=bossId=0
    pt=0
    # print "parseEnemyTable table:"+table

    tr, pt = substr('<tr', '</tr', table, pt)
    enemyItems=bossItems=""
    while True:
        tr, pt = substr('<tr', '</tr', table, pt)
        if tr=='':
            break
        p=0
        # print "parseEnemyTable tr:"+tr
        td1, p = substr('<td>', '</td', tr, p)
        td2, p = substr('<td>', '</td', tr, p)
        td3, p = substr('<td>', '</td', tr, p)
        td4, p = substr('<td>', '</td', tr, p)
        td5, p = substr('<td>', '</td', tr, p)
        td6, p = substr('<td>', '</td', tr, p)
        td7, p = substr('<td>', '</td', tr, p)
        td8, p = substr('<td>', '</td', tr, p)
        # print enemyType+" td2:" + td2
        # print "td4:" + td4
        # print "td5:" + td5
        # print "td6:" + td6
        # print "td7:" + td7
        # print "td8:" + td8
        id, t = substr('title="ID:',' ', td2, 0)
        unitId=trimNumber(id)
        if unitId=="":
            print "ERROR: unit is EMPTY.   \n td2: %s" % (td2)
            print "table:\n"+table
            continue
        atk = td4
        next = td5
        hp = td6
        defence = td7
        print "parseEnemyTable unitId:%s nameId:%d" % (unitId,nameId)
        # print "before get dropRateMap.keys:", dropRateMap.keys()
        # print "before get dropRateMap.values:", dropRateMap.values()
        dropRate="不適用"
        dropRate=dropRateMap.get(unitId)

        dropId="0"
        dropLv="0"
        if td8.find('title="ID:') > 0:
            dropId, t = substr('title="ID:',' ', td8, 0)
            dropLv, t = substr('</small>','<', td8, t)
            dropId=trimNumber(dropId)
            dropLv=dropLv.strip()

        print "id:%s atk:%s, next:%s, hp:%s, defence:%s, dropId:%s, dropLv:%s" % (id,atk,next,hp,defence,dropId,dropLv)

        if td1.find("Boss")>=0:
            bossId+=1
            line = '"%s","%s","%s","%s","%s","%s","%s","%s","%s","%s",\n' % (nameId,bossId,unitId,atk,next,hp,defence,dropId,dropLv,dropRate)
            bossItems+=line
        else:
            enemyId=enemyId+1
            line = '"%s","%s","%s","%s","%s","%s","%s","%s","%s","%s",\n' % (nameId,enemyId,unitId,atk,next,hp,defence,dropId,dropLv,dropRate)
            enemyItems+=line
    return enemyItems, bossItems

def parseEnemyTable4Droprate(nameId, enemyType, table):
    # filename = "./%s_%d.csv" % (enemyType, nameId)
    # fileSave = open(filename, "w+")
    # fileSave.write('"enemyId","unitId","atk","next","hp","defence","dropId","dropLv",\n')

    print("parseEnemyTable4Droprate  parse table:"+enemyType)
    # print("table:"+table)
    enemyId=0
    pt=0
    tr, pt = substr('<tr', '</tr', table, pt)
    dropMap={}
    while True:
        tr, pt = substr('<tr', '</tr', table, pt)
        if tr=='':
            break
        p=0
        # print "tr:"+tr
        # continue
        td1, p = substr('<td>', '</td', tr, p)
        td2, p = substr('<td>', '</td', tr, p)
        td3, p = substr('<td>', '</td', tr, p)
        td4, p = substr('<td>', '</td', tr, p)
        td5, p = substr('<td>', '</td', tr, p)
        td6, p = substr('<td>', '</td', tr, p)
        td7, p = substr('<td>', '</td', tr, p)


        # print enemyType+" td1:" + td1
        # print enemyType+" td2:" + td2
        # print "td4:" + td4
        # print "td5:" + td5
        # print "td6:" + td6
        # print "td7:" + td7
        # print "td8:" + td8
        id, t = substr('<img alt="Id','.', td1, 0)
        unitId=trimNumber(id)
        if unitId=='':
            continue

        dropRate=td7
        if dropRate[len(dropRate)-1:]=="%":
            dropRate=dropRate[:len(dropRate)-1]

        enemyId=enemyId+1
        line = '"%s","%s","%s"' % (enemyId,unitId, dropRate)
        print "add dropRate: %s -> %s" % (unitId, dropRate)
        # dropMap[str(unitId)]=dropRate
        dropMap.setdefault(unitId, dropRate)

        print line
        # fileSave.write(line+"\n")

    # print "keys:", dropMap.keys()
    # print "values:", dropMap.values()

    return dropMap

def ParseQuestInfo(table):
    pt=0
    tr1,pt=substr("<tr","</tr", table, pt)
    tr2,pt=substr("<tr","</tr", table, pt)
    pt=0
    td1,pt=substr("<td","/td", tr1, pt)
    td2,pt=substr("<td","/td", tr1, pt)
    td3,pt=substr("<td","/td", tr1, pt)
    td4,pt=substr("<td","/td", tr1, pt)
    stamina,t=substr(">","<", td2, 0)
    span,t=substr("<span","</span", td4, 0)
    coin,t=substr(">","<", span, 0)
    coin=trimNumber(coin)
    # ticket,t=substr(">","<", td3, 0)
    pt=0
    td1,pt=substr("<td","/td", tr2, pt)
    td2,pt=substr("<td","/td", tr2, pt)
    td3,pt=substr("<td","/td", tr2, pt)
    td4,pt=substr("<td","/td", tr2, pt)
    floor,t=substr(">","<", td2, 0)
    exp,t=substr(">","<", td4, 0)
    # print ">>>>>>> exp.... td2:"+td2
    print "stamina:%s,coin:%s,exp:%s,floor:%s" % (stamina,coin,exp,floor)
    return stamina,coin,exp,floor

def ParseStar(questId, tr):
    p=0
    td1,p=substr('<td', '/td', tr, p)
    td2,p=substr('<td', '/td', tr, p)
    td3,p=substr('<td', '/td', tr, p)
    td4,p=substr('<td', '/td', tr, p)

    # print "td1:%s" % td1
    # print "td2:%s" % td2

    star=""
    p=0
    if td1.find("!")>=0:
        star="!"
    else:
        star,p=substr('>', '★', td1, 0)
    starNum,p=substr('×', '<', td1, p)
    treasure,p=substr('>', '<', td2, 0)
    treasure=re.sub(' ','',treasure)
    treasure=re.sub(',', '', treasure) #remove number splitter, eg. 2,300
    treasure=re.sub('–', '|', treasure)

    # print "td3:%s" % td3
    # trap=td3

    trap=""
    p=0
    while True:
        trapItem, p=substr('<abbr class=','</abbr', td3, p)
        if trapItem=='':
            break

        trapLv="1"
        if trapItem.find('trap-level-')>=0:
            trapLv, tmp = substr('trap-level-', '"', trapItem,0)
        trapId, tmp=substr('data-image-key="Trap', '.', trapItem, 0)

        trap=trap+trapId+":"+trapLv+"|"
        # print "[%d] trap:%s" % (i, trap)

    # print 'td4:"%s"' % td4

    p=0
    enemyPool=""
    enemyNum="0|0"

    tmpStr, t=substr('>', '<span', td4, p)
    if tmpStr != "":
        numRange, t=substr('', '×', tmpStr, p)

        if numRange.find("–")>0:
            ss=numRange.split("–")
            numRange=ss[0]+"|"+ss[1]
        else:
            if numRange != "":
                numRange=numRange+"|"+numRange
        enemyNum=numRange

    # print "enemyNum:%s" % enemyNum

    while True:
        span,p=substr('<span', '</span', td4,p)
        if span == '':
            break

        enemy, t=substr('title="ID:', ' ', span, 0)
        enemy=trimNumber(enemy)
        enemyPool=enemyPool+enemy+"|"

    if enemyPool!="": #remove last ","
        enemyPool=enemyPool[:len(enemyPool)-1]

    print "star:%s starNum:%s treasure:%s trap:%s enemyPool:'%s' enemyNum:'%s' "\
          % (star,starNum,treasure,trap,enemyPool, enemyNum)
    result='"%s","%s","%s","%s","%s","%s","%s",' % (questId,star,starNum,treasure,trap,enemyPool, enemyNum)
    return result

def ParseStageDetail(stageId, html):
    pt=0
    questNo=0
    detailMap={}
    while True:
        table, pt=substr('<table class="wikitable">', "</table", html, pt)
        if table=='':
            break
        questNo = questNo + 1
        questId = stageId*10 + questNo
        detailMap.setdefault(questId, table)

    return detailMap


def ParseQuestDetail(table, detail, questId):
    pt=0
    table=detail[questId]

    # tr0,pt=substr("<tr","</tr", table, pt)
    # print "tr0:"+tr0
    th,pt=substr("<tr","</tr", table, pt)
    # print "th:"+th
    tr1,pt=substr("<tr","</tr", table, pt)
    tr2,pt=substr("<tr","</tr", table, pt)
    tr3,pt=substr("<tr","</tr", table, pt)
    tr4,pt=substr("<tr","</tr", table, pt)
    tr5,pt=substr("<tr","</tr", table, pt)
    tr6,pt=substr("<tr","</tr", table, pt)
    tr7,pt=substr("<tr","</tr", table, pt)

    p=0
    keyNum,p=substr('×', '<', th, p)
    treasureNum,p=substr('×', '<',th, p)
    trapNum,p=substr('×', '<', th, p)
    enemyNum,p=substr('×', '<',th, p)
    keyNum=trimNumber(keyNum)
    # print "keyNumkeyNum:"+keyNum
    treasureNum=trimNumber(treasureNum)
    trapNum=trimNumber(trapNum)
    enemyNum=trimNumber(enemyNum)
    print "keyNum:%s,treasureNum:%s,trapNum:%s,enemyNum:%s" % (keyNum,treasureNum,trapNum,enemyNum)


    summary='"questId","keyNum","treasureNum","trapNum","enemyNum",\n'
    summary=summary+('"%s","%s","%s","%s","%s",\n\n' % (questId,keyNum,treasureNum,trapNum,enemyNum))

    header='"questId","star","starNum","treasure","trap","enemyPool","enemyNum"\n'
    result=summary+header
    result=result+ParseStar(questId, tr1)+"\n"
    result=result+ParseStar(questId, tr2)+"\n"
    result=result+ParseStar(questId, tr3)+"\n"
    result=result+ParseStar(questId, tr4)+"\n"
    result=result+ParseStar(questId, tr5)+"\n"
    result=result+ParseStar(questId, tr6)+"\n"
    result=result+ParseStar(questId, tr7)+"\n\n"

    return result

def ParseColorPercent(table, cityId, stageId, savepath):
    pt=0
    tr, pt = substr('<tr', '</tr', table, pt)
    tr, pt = substr('<tr', '</tr', table, pt)
    tr, pt = substr('<tr', '</tr', table, pt)
    tr, pt = substr('<tr', '</tr', table, pt)
    tr1, pt = substr('<tr', '</tr', table, pt)
    tr2, pt = substr('<tr', '</tr', table, pt)
    # tr3, pt = substr('<tr', '</tr', table, pt)

    td1, t = substr('<td', '</td', tr1, 0)
    td2, t = substr('<td', '</td', tr1, t)
    td3, t = substr('<td', '</td', tr1, t)
    td4, t = substr('<td', '</td', tr1, t)
    td5, t = substr('<td', '</td', tr1, t)
    td6, t = substr('<td', '</td', tr1, t)
    td7, t = substr('<td', '</td', tr1, t)

    td21, t = substr('<td', '/td', tr2, 0)
    td22, t = substr('<td', '/td', tr2, t)
    td23, t = substr('<td', '/td', tr2, t)
    td24, t = substr('<td', '/td', tr2, t)
    td25, t = substr('<td', '/td', tr2, t)
    td26, t = substr('<td', '/td', tr2, t)
    td27, t = substr('<td', '/td', tr2, t)

    colorPercent={}
    colorType, t = substr('<img alt="','"', td1, 0)

    percent, t = substr('>', '<', td21, 0)
    colorPercent[colorType] = percent
    # print "color:%s => percent:%s" % (colorType, percent)

    colorType, t = substr('<img alt="','"', td2, 0)
    percent, t = substr('>', '<', td22, 0)
    colorPercent[colorType] = percent

    colorType, t = substr('<img alt="','"', td3, 0)
    noused, t = substr('<noscript','</noscript',td3,t)
    percent, t = substr('>', '<', td23, 0)
    colorPercent[colorType] = percent

    colorType, t = substr('<img alt="','"', td4, 0)
    noused, t = substr('<noscript','</noscript',td4,t)
    percent, t = substr('>', '<', td24, 0)
    colorPercent[colorType] = percent

    colorType, t = substr('<img alt="','"', td5, 0)
    noused, t = substr('<noscript','</noscript',td5,t)
    percent, t = substr('>', '<', td25, 0)
    colorPercent[colorType] = percent

    colorType, t = substr('<img alt="','"', td6, 0)
    noused, t = substr('<noscript','</noscript',td6,t)
    percent, t = substr('>', '<', td26, 0)
    colorPercent[colorType] = percent

    colorType, t = substr('<img alt="','"', td7, 0)
    noused, t = substr('<noscript','</noscript',td7,t)
    percent, t = substr('>', '<', td27, 0)
    colorPercent[colorType] = percent

    # header=line=''
    # for color in colorPercent:
    #     header=header + '"'+color +'",'
    #     percent=re.sub('%', '', colorPercent[color])
    #     line=line + '"' + percent +'",'
    # print "color percent:"+line

    header='"stageId","风","火","水","光","闇","无","心",'
    line='"'+colorPercent["风"]+'",'+'"'+colorPercent["火"]+'",'+'"'+colorPercent["水"]+'",' \
         +'"'+colorPercent["光"]+'",'+'"'+colorPercent["闇"]+'",'+'"'+colorPercent["无"]+'",' \
         +'"'+colorPercent["心"]+'",'
    line='"%d",%s' % (stageId, line)
    fileSave = open(savepath+"/color_%d.csv" % (cityId), "a+")
    fileSave.write(header+'\n')
    line=re.sub('%','',line)
    fileSave.write(line+'\n')
    fileSave.write('\n')
    fileSave.close()
    print "colorPercent:", colorPercent

    return

def ParseStagePage(cityId, stageId, html, detail):
    savepath="%s/%d" % (dataPath, cityId)
    os.system('mkdir -p '+savepath)

    pos=pt=0
    print "begin parse GetDropRate(%d)..." % (stageId)
    # enemyDrop={}
    # bossDropList=[{},{},{},{},{},{},{},{}]
    enemyDrop, bossDropList=GetDropRate(stageId)

    table, t = substr('<table class="module move WikiaArticle area-info', '</table', html, 0)

    ParseColorPercent(table, cityId, stageId, savepath)


    html, t = substr('<div id="mw-content-text" ', '<nav class="WikiaArticleCategories', html, pos)

    h2, pos = substr('<h2>', '</h2', html, pos)
    h2, pos = substr('<h2>', '</h2', html, pos)
    if h2.find("乱入")>0:
        h2, pos = substr('<h2>', '</h2', html, pos)

    # enemy list
    table, pos = substr('<table class="wikitable enemy-table"', '</table', html, pos)
    # print "table:"+table

    result=parseEnemyTable(stageId, "enemy", table, enemyDrop)
    fileSave = open(savepath+"/enemylist_%d.csv" % (cityId), "a+")
    fileSave.write('"stageId","enemyId","unitId","atk","next","hp","defence","dropId","dropLv","dropRate",\n')
    fileSave.write(result)
    fileSave.write('\n')
    fileSave.close()
    print "enemylist_%d: %s" % (stageId,result)

    bossItems=questInfos=starinfo=""
    questId=stageId*10
    while True: #quest info
        h2,pos=substr("<h2>","</h2", html, pos)
        if h2.find('<span class="mw-headline"') <0:
            # print "h2.find(mw-headline) fail. break..."+h2
            break
        if h2.find('乱入') >0:
            continue

        bossDrop=bossDropList[questId%10]
        questId=questId+1

        print "processing questId:%d questId%%10:%d" % (questId, questId%10)
        print "h2:"+h2

        #quest info
        table,pos=substr("<table","</table", html, pos)
        stamina,coin,exp,floor=ParseQuestInfo(table)
        questInfo='"%s","%s","%s","%s","%s",\n' % (questId,stamina,coin,exp,floor)
        questInfos=questInfos+questInfo
        print "questInfo:"+questInfo


        # boss info
        tableBoss,pos=substr("<table","</table", html, pos)
        bossItem=parseEnemyTable(questId, "boss", tableBoss, bossDrop)
        bossItems=bossItems+bossItem
        print "bossItems_%d: %s" % (questId,bossItems)

        #not used
        table,pos=substr("<table","</table", html, pos)

        #详细数据
        stars=ParseQuestDetail(table, detail, questId)
        starinfo=starinfo+stars

        print "questId:%d finish." % (questId)
        print "===========================================\n"

    fileSave = open(savepath+"/bosslist_%d.csv" % (cityId), "a+")
    fileSave.write('"questId","enemyId","unitId","atk","next","hp","defence","dropId","dropLv","dropRate",\n')
    fileSave.write(bossItems)
    fileSave.write('\n')
    fileSave.close()

    fileSave = open(savepath+"/questinfo_%d.csv" % (cityId), "a+")
    fileSave.write('"questId","stamina","coin","exp","floor",\n')
    fileSave.write(questInfos)
    fileSave.write('\n')
    fileSave.close()

    fileSave = open(savepath+"/starinfo_%d.csv" % (stageId), "w+")
    fileSave.write(starinfo)
    fileSave.close()

    return

def ParseStageNew(cityId, stageId, html, detail):
    savepath="%s/%d" % (dataPath, cityId)
    os.system('mkdir -p '+savepath)

    pos=pt=0
    print "begin parse GetDropRate(%d)..." % (stageId)
    enemyDrop={}
    bossDropList=[{},{},{},{},{},{},{},{}]
    # enemyDrop, bossDropList=GetDropRate(stageId)

    table, t = substr('<table class="module move WikiaArticle area-info', '</table', html, 0)

    ParseColorPercent(table, cityId, stageId, savepath)

    html, t = substr('<div id="mw-content-text" ', '<nav class="WikiaArticleCategories', html, pos)

    pos=0
    #skip h2=目录
    h2, pos = substr('<h2>', '</h2', html, pos)
    # h2, pos = substr('<h2>', '</h2', html, pos)
    # if h2.find("乱入")>0:
    #     h2, pos = substr('<h2>', '</h2', html, pos)


    enemyItems=bossItems=questInfos=starinfo=""
    questId=stageId*10
    while True: #quest info
        h2,pos=substr("<h2>","</h2", html, pos)
        if h2.find('<span class="mw-headline"') <0:
            # print "h2.find(mw-headline) fail. break..."+h2
            break
        if h2.find('乱入') >0:
            continue

        bossDrop=bossDropList[questId%10]
        questId=questId+1

        print "processing questId:%d questId%%10:%d" % (questId, questId%10)
        print "h2:"+h2

        #quest info
        table,pos=substr("<table","</table", html, pos)
        stamina,coin,exp,floor=ParseQuestInfo(table)
        questInfo='"%s","%s","%s","%s","%s",\n' % (questId,stamina,coin,exp,floor)
        questInfos=questInfos+questInfo
        print "questInfo:"+questInfo

        # enemy list
        table, pos = substr('<table class="wikitable enemy-table"', '</table', html, pos)
        # print "table:"+table

        enemyItem,bossItem=parseEnemyTable(questId, "enemy", table, enemyDrop)
        enemyItems+=enemyItem
        bossItems+=bossItem
        print "questId:%d enemyItem:%s" % (questId, enemyItem)
        print "questId:%d bossItem:%s" % (questId, bossItem)

        #not used
        # table,pos=substr("<table","</table", html, pos)

        #详细数据
        stars=ParseQuestDetail(table, detail, questId)
        starinfo=starinfo+stars

        print "questId:%d finish." % (questId)
        print "===========================================\n"

    fileSave = open(savepath+"/enemylist_%d.csv" % (cityId), "a+")
    fileSave.write('"questId","enemyId","unitId","atk","next","hp","defence","dropId","dropLv","dropRate",\n')
    fileSave.write(enemyItems)
    fileSave.write('\n')
    fileSave.close()
    print "enemylist_%d: %s" % (stageId,enemyItems)

    fileSave = open(savepath+"/bosslist_%d.csv" % (cityId), "a+")
    fileSave.write('"questId","enemyId","unitId","atk","next","hp","defence","dropId","dropLv","dropRate",\n')
    fileSave.write(bossItems)
    fileSave.write('\n')
    fileSave.close()

    fileSave = open(savepath+"/questinfo_%d.csv" % (cityId), "a+")
    fileSave.write('"questId","stamina","coin","exp","floor",\n')
    fileSave.write(questInfos)
    fileSave.write('\n')
    fileSave.close()

    fileSave = open(savepath+"/starinfo_%d.csv" % (stageId), "w+")
    fileSave.write(starinfo)
    fileSave.close()

    return

def GetDroprateStageUrl(stageId):
    count=stageId/10-1
    no=stageId%10
    count=count*7+no-1

    # #御加城-童话系列
    # stageUrls=[]
    # stageUrls.append("http://www.divinegatewiki.com/wiki/%E5%BE%A1%E4%BC%BD%E5%9F%8E%E3%82%A2%E3%82%AB%E3%82%BA%E3%82%AD%E3%83%B3")
    # stageUrls.append("http://www.divinegatewiki.com/wiki/%E5%BE%A1%E4%BC%BD%E5%9F%8E%E3%82%A2%E3%83%AA%E3%82%B9")
    # stageUrls.append("http://www.divinegatewiki.com/wiki/%E5%BE%A1%E4%BC%BD%E5%9F%8E%E3%82%A4%E3%83%90%E3%83%A9")
    # stageUrls.append("http://www.divinegatewiki.com/wiki/%E5%BE%A1%E4%BC%BD%E5%9F%8E%E3%82%B7%E3%83%B3%E3%83%87%E3%83%AC%E3%83%A9")
    # stageUrls.append("http://www.divinegatewiki.com/wiki/%E5%BE%A1%E4%BC%BD%E5%9F%8E%E3%82%AB%E3%82%B0%E3%83%A4")
    # stageUrls.append("http://www.divinegatewiki.com/wiki/%E5%BE%A1%E4%BC%BD%E5%9F%8E%E3%82%B7%E3%83%A9%E3%83%A6%E3%82%AD")
    # return stageUrls[stageId%10-1]


    # for k, url in enumerate(droprateStageUrls):
    #     if k < count:
    #         continue
    if count >= len(droprateStageUrls):
        return ""

    print "droprateStageUrls:%d url:%s" % (count, droprateStageUrls[count])
    return 'http://www.divinegatewiki.com'+droprateStageUrls[count]


def GetDropRate(stageId):

    enemyDrop={}
    bossDrop=[]

    url=GetDroprateStageUrl(stageId)
    if url == "":
        enemyDrop={}
        bossDrop=[{},{},{},{},{},{},{},{}]
        print "Cannot GetDropUrl for stage:%d" % (stageId)
        return enemyDrop, bossDrop

    stageUrl=url
    print "wget for droprate: %s" % (stageUrl)

    request=urllib2.Request(stageUrl)
    html=urllib2.urlopen(request).read()
    body, p=substr('<div id="mw-content-text" lang="zh-HK"', '<div class="printfooter">', html,0)
    pos=0
    h2,pos=substr("<h2>","</h2", body, pos)
    # print "1------ h2: "+h2
    h2,pos=substr("<h2>","</h2", body, pos)
    if h2.find("任務資訊")>0:
        print "UNNORMAL h2: "+h2
        # h2,pos=substr("<h2>","</h2", body, pos)
        # h2,pos=substr("<h2>","</h2", body, pos)

    # print "2------ h2: "+h2

    table,pos=substr("<table","</table", body, pos)
    # print "3------ table: "+table
    table,pos=substr("<table","</table", body, pos)
    # print "4------ table: "+table
    if table.find('日期')>0:
        table,pos=substr("<table","</table", body, pos)
    enemyDrop=parseEnemyTable4Droprate(stageId,"enemy",table)
    # return

    questNo=0
    while True:
        h2,pos=substr("<h2>","</h2", body, pos)
        if h2=='':
            break
        if h2.find('<span class="mw-headline"')>=0:
            table,pos=substr("<table","</table", body, pos)
            table,pos=substr("<table","</table", body, pos)
            # table,pos=substr("<table","</table", body, pos)

            questNo=questNo+1
            print "parse rate for boss:"
            dropMap=parseEnemyTable4Droprate(stageId,"boss",table)
            # for k,v in enumerate(dropMap):
            #     print "\t\tbossDropRate :: %d %s" % (k, v)
            bossDrop.append(dropMap)

    return enemyDrop, bossDrop


def ParseIndexPage(CityId):
    # f = open(INDEX_FILE,'r')
    # html = f.read()
    # f.close
    stageId=10*CityId # 20:第2系列

    cityurl=[]
    #都之系列
    cityurl.append('http://zh.divine-gate.wikia.com/wiki/%E9%83%BD%E4%B9%8B%E7%B3%BB%E5%88%97')
    #監獄系列
    cityurl.append('http://zh.divine-gate.wikia.com/wiki/%E7%9B%A3%E7%8D%84%E7%B3%BB%E5%88%97')
    #塔之系列
    cityurl.append('http://zh.divine-gate.wikia.com/wiki/%E5%A1%94%E4%B9%8B%E7%B3%BB%E5%88%97')
    #楼阁系列
    cityurl.append('http://zh.divine-gate.wikia.com/wiki/%E6%A8%93%E9%96%A3%E7%B3%BB%E5%88%97')
    #休息室
    cityurl.append('http://zh.divine-gate.wikia.com/wiki/%E4%BC%91%E6%81%AF%E5%AE%A4%E7%B3%BB%E5%88%97')
    #研究所
    cityurl.append('http://zh.divine-gate.wikia.com/wiki/%E7%A0%94%E7%A9%B6%E6%89%80%E7%B3%BB%E5%88%97')
    #游园地
    cityurl.append('http://zh.divine-gate.wikia.com/wiki/%E9%81%8A%E5%9C%92%E5%9C%B0%E7%B3%BB%E5%88%97')

    request=urllib2.Request(cityurl[CityId-1] + "?variant=zh-hans")
    html=urllib2.urlopen(request).read()
    div,pt=substr('<div id="mw-content-text"','</ul>', html, 0)
    pt=0

    savepath= "%s/%s" % (dataPath, CityId)
    os.system('rm -vf '+savepath+"/enemylist_*")
    os.system('rm -vf '+savepath+"/bosslist_*")
    os.system('rm -vf '+savepath+"/questinfo_*")
    os.system('rm -vf '+savepath+"/starinfo_*")
    os.system('rm -vf '+savepath+"/color_*")

    while True:
        stageId=stageId+1
        li, pt=substr('<li','</li>', div, pt)
        if li == '':
            break
        href, p=substr('href="','"', li, 0)

        if stageId==36:
            href='/wiki/水晶塔クォーツ'

        mainlink = "http://zh.divine-gate.wikia.com" + href + "?variant=zh-hans"
        detaillink = "http://zh.divine-gate.wikia.com" + href + "/詳細資料?variant=zh-hans"
        print "[%d] link:%s" % (stageId, mainlink)

        html=urllib2.urlopen(mainlink).read()
        print "[%d] detail link:%s" % (stageId, detaillink)
        detail=urllib2.urlopen(detaillink).read()

        detail=ParseStageDetail(stageId,detail)
        ParseStagePage(CityId, stageId, html, detail)

        # break

def ParseStages(CityId, stageUrls):
    savepath= "%s/%s" % (dataPath, CityId)
    os.system('rm -vf '+savepath+"/enemylist_*")
    os.system('rm -vf '+savepath+"/bosslist_*")
    os.system('rm -vf '+savepath+"/questinfo_*")
    os.system('rm -vf '+savepath+"/starinfo_*")
    os.system('rm -vf '+savepath+"/color_*")

    pos=0
    # CityId=101 #egg city
    stageNo=0
    while stageNo<len(stageUrls):
        mainlink = stageUrls[stageNo] + "?variant=zh-hans"
        detaillink = stageUrls[stageNo] + "/詳細資料?variant=zh-hans"

        stageNo+=1
        stageId=CityId*10 + stageNo

        print "Evo stage[%d] link:%s" % (stageId, mainlink)

        html=urllib2.urlopen(mainlink).read()
        print "Evo stage[%d] detail link:%s" % (stageId, detaillink)
        detail=urllib2.urlopen(detaillink).read()

        detail=ParseStageDetail(stageId, detail)
        ParseStageNew(CityId, stageId, html, detail)

        # break

###===========================================================================###
def ParseEvolveCity(CityId):
    stageUrl=[]
    stageUrl.append('http://zh.divine-gate.wikia.com/wiki/%E7%83%88%E7%81%AB%E6%AE%BF%E3%83%98%E3%83%91%E3%82%A4%E3%82%B9%E3%83%88%E3%82%B9')
    stageUrl.append('http://zh.divine-gate.wikia.com/wiki/%E6%B5%81%E6%B0%B4%E6%AE%BF%E3%83%9D%E3%82%BB%E3%82%A4%E3%83%89%E3%83%B3')
    stageUrl.append('http://zh.divine-gate.wikia.com/wiki/%E6%97%8B%E9%A2%A8%E6%AE%BF%E3%83%98%E3%83%AB%E3%83%A1%E3%82%B9')
    stageUrl.append('http://zh.divine-gate.wikia.com/wiki/%E9%96%83%E5%85%89%E6%AE%BF%E3%82%A2%E3%83%AB%E3%83%86%E3%83%9F%E3%82%B9')
    stageUrl.append('http://zh.divine-gate.wikia.com/wiki/%E5%B8%B8%E9%97%87%E6%AE%BF%E3%83%8F%E3%83%BC%E3%83%87%E3%82%B9')
    stageUrl.append('http://zh.divine-gate.wikia.com/wiki/%E7%B5%B6%E7%84%A1%E6%AE%BF%E3%83%98%E3%82%B9%E3%83%86%E3%82%A3%E3%82%A2')
    ParseStages(CityId, stageUrl)

def ParseCoinCity(CityId):
    CoinStageUrls=[]
    CoinStageUrls.append("http://zh.divine-gate.wikia.com/wiki/%E9%BB%84%E9%87%91%E9%83%B7%E3%82%A8%E3%83%AB%E3%83%89%E3%83%A9%E3%83%89%E2%85%A0")
    CoinStageUrls.append("http://zh.divine-gate.wikia.com/wiki/%E9%BB%84%E9%87%91%E9%83%B7%E3%82%A8%E3%83%AB%E3%83%89%E3%83%A9%E3%83%89%E2%85%A1")
    ParseStages(CityId, CoinStageUrls)

def ParseEvent1(CityId):
    StageUrls=[]
    StageUrls.append("http://zh.divine-gate.wikia.com/wiki/%E5%BE%A1%E4%BC%BD%E5%9F%8E%E3%82%A2%E3%82%AB%E3%82%BA%E3%82%AD%E3%83%B3")
    StageUrls.append("http://zh.divine-gate.wikia.com/wiki/%E5%BE%A1%E4%BC%BD%E5%9F%8E%E3%82%A2%E3%83%AA%E3%82%B9")
    StageUrls.append("http://zh.divine-gate.wikia.com/wiki/%E5%BE%A1%E4%BC%BD%E5%9F%8E%E3%82%A4%E3%83%90%E3%83%A9")
    StageUrls.append("http://zh.divine-gate.wikia.com/wiki/%E5%BE%A1%E4%BC%BD%E5%9F%8E%E3%82%B7%E3%83%B3%E3%83%87%E3%83%AC%E3%83%A9")
    StageUrls.append("http://zh.divine-gate.wikia.com/wiki/%E5%BE%A1%E4%BC%BD%E5%9F%8E%E3%82%AB%E3%82%B0%E3%83%A4")
    StageUrls.append("http://zh.divine-gate.wikia.com/wiki/%E5%BE%A1%E4%BC%BD%E5%9F%8E%E3%82%B7%E3%83%A9%E3%83%A6%E3%82%AD")
    ParseStages(CityId, StageUrls)



#ParseEvolveCity(100) # 100:进化神殿
# ParseEgg(101)      # 101:蛋本
# ParseCoinCity(102)   # 102:黄金乡
ParseEvent1(111) #第一个单周活动
