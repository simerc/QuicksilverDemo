(function(b,a){b.create("tinymce.plugins.epifilebrowser",{init:function(f,c){if(typeof EPi==="undefined"||typeof EPi.ResolveUrlFromUI!="function"){return;}var d=function(g,j){if(g&&g.items&&g.items.length===1){var k=j.window.document;var i=k.getElementById(j.formInputId);var l=g.items[0];if(i.value===l.path){return;}i.value=l.path;if(i.onchange){i.onchange(i.value);}if(j.formInputId==="src"){var h=k.getElementById("alt");if(h!=null&&typeof(l.description)==="string"&&!h.value){h.value=l.description;}}}f.windowManager.onClose.dispatch();};var e=function(l,g,i,j){var k=f.settings.epi_page_context;var h=a.extend({},k,{hideclearbutton:true,browserselectionmode:i,selectedfile:g});var m=EPi.ResolveUrlFromUI("edit/FileManagerBrowser.aspx")+"?"+a.param(h);f.windowManager.onOpen.dispatch();EPi.CreateDialog(m,d,{window:j,formInputId:l},h,{width:800,height:650},j);};f.settings.file_browser_callback=e;},getInfo:function(){return{longname:"File Browser Plug-In",author:"EPiServer AB",authorurl:"http://www.episerver.com",infourl:"http://www.episerver.com",version:1};}});b.PluginManager.add("epifilebrowser",b.plugins.epifilebrowser);}(tinymce,epiJQuery));