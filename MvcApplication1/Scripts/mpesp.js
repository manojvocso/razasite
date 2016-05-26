

var MP={
<!-- mp_trans_disable_start -->
	Version: '1.0.14',
	Domains:{'es':'espanol.verizon.com'},
	SrcLang: 'en',
<!-- mp_trans_disable_end -->
	UrlLang: 'mp_js_current_lang',
	SrcUrl: unescape('mp_js_orgin_url'),
<!-- mp_trans_disable_start -->
	init: function(){
		if(MP.UrlLang.length!=2){
			MP.UrlLang=MP.SrcLang;
		}
		if(MP.SrcUrl.indexOf('p_js_orgin_url')==1){
			MP.SrcUrl=location.href;
		}
	},
	switchLanguage: function(lang){
		if(lang!=MP.SrcLang){
			var script=document.createElement('SCRIPT');
			script.src=location.protocol+'//'+MP.Domains[lang]+'/en'+lang+'/?1023749632;'+escape(MP.SrcUrl);
			document.body.appendChild(script);
		}else if(lang==MP.SrcLang){
			MP.switchToLang(MP.SrcUrl);
		}
		return false;
	},
	switchToLang: function(url){
		var mplink=document.createElement('A');
		if(mplink.click){// using location.href will cause IE6 to not report referrer
			mplink.href=url;
			document.body.appendChild(mplink);
			mplink.click();
		}else{
			location.href=url;
		}
	}
<!-- mp_trans_disable_end -->
};
