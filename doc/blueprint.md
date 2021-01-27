



需要记录的图书信息：

1. 标题
2. 副标题
3. 作者
4. 译者
5. 作者简介
6. 出版社
7. 版次
8. 字数
9. 起始阅读日期
10. 终止阅读日期
11. 评分
12. 评语
13. ISBN
14. 类别【技术/通识】



软件主界面分为左右两栏。左栏为主要区域，用于展示



XML格式定义：

```xml
<readrco>
	<record>
    	<id>1</id>
        <book>
        	<main_title>老后破产</main_title>
            <sub_title>名为“长寿”的噩梦</sub_title>
            <authors>
            	<author>NHK特别节目录制组</author>
                <author>假想的人</author>
            </authors>
            <translators>
            	<translator>王军</translator>
            </translators>
            <press>上海译文出版社</press>
            <press_sn>201807</press_sn>
            <word_count>87</word_count>
            
        </book>
        <read_info>
            <status>1</status>
        	<begin_date>2021-01-18</begin_date>
            <end_date>2021-01-26</end_date>
            <star>5</star>
            <comment>长寿不是噩梦，贫穷是原罪。</comment>
        </read_info>
    </record>
</readrco>
```



