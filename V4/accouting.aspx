<%@ Page Language="VB" AutoEventWireup="false" CodeFile="accouting.aspx.vb" Inherits="accouting" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head >
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css" />

<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script src="jqq/ui/jquery.ui.progressbar.js"></script>
	<script src="jqq/ui/jquery.ui.datepicker.js"></script>
	<script src="jqq/ui/jquery.ui.button.js"></script>
	<script src="jqq/ui/jquery.ui.dialog.js"></script>
	
	<link rel="stylesheet" href="jq/demos.css">
	<script type="text/javascript" src="scripts/form.js"></script>

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
<style type="text/css">
    .c1
    {
    	width:400px;
    	float:left;
    	background:#ffffbd;
    	height:500px;
    	    	overflow:auto;
    	
    	}
    	.c2
    {
    	width:300px;
    	float:left;
    	background:#ffcdff;
    	height:500px;
    	overflow:auto;
    	
    	}
    	.c3
    {
    	width:400px;
    	float:left;
    	background:#ffbbaa;
    	height:500px;
    	overflow:auto;
    	
    	
    	}
    	.sp
    	{
    		width:10px;
    		float:left;
    	
    		}

</style>
<script>
    function showHideSubMenu(link, id) {
    
        var uldisplay = "";
        var newClass = "";
        var idlink = "";
        var parent 
        parent= link.parentNode;
        var spanx; 
        spanx = parent.getElementsByTagName('span');
        alert(id);
        if (link.className == 'expanded') {

            // Need to hide
            
            uldisplay = 'none';
            newClass = 'collapsed';
            
            $(spanx[1]).css({ background : 'url(images/gif/collapsed.gif) no-repeat' });
            
        } else {
            // Need to show
            uldisplay = 'block';
            newClass = 'expanded';
           
          $(spanx[1]).css({ background: 'url(images/gif/expanded.gif) no-repeat' });
        }


        $("#" + id).css({ 'display': uldisplay });
        link.className = newClass;
        
    }
    function seenon(id, bywho, datex) {
        var sql;
        sql = "update rptdataupdate set seen=1, seendate='" + datex + "',seenby='" + bywho + "' where id=" + id;
        $("#fpay").attr("frameborder", "0");
        $('#frmx').attr("target", "fpay");
        $('#frmx').attr("action", "frmdatasaveonly.aspx?sql=" + sql);
        $('#frmx').submit();
        $('#pay').css({ top: '0px', left: '0px' });
        $("#pay").remove("display");
        $("#pay").dialog({
            top: 30,
            title: 'Bank St.',
            height: 500,
            width: 800,
            modal: true
        });

        $("#fpay").css({ width: '800px' });
    }
    function closedx(id) {
        var sql;
        sql = "update emp_resign set active='n' where id="+id;
        $('#frmx').attr("target", "fpay");
        $("#fpay").attr("frameborder", "0");

        $('#frmx').attr("action", "frmdatasaveonly.aspx?sql=" + sql+"&tar=accouting.aspx");
        $('#frmx').submit();
       // $('#post').attr("disabled", "disabled");
        $('#pay').css({ top: '0px', left: '0px' });
        $("#pay").remove("display");
        $("#pay").dialog({
            title: 'Data Saving',
            height: 300,
            width: 600,
            modal: true
        });


    }
    function balancep(id) { 
    
    }
</script>

    <link href="css/payrol.css" media="screen" rel="stylesheet" type="text/css" />
    <title>Untitled Page</title>
</head>
<body>
<%  Dim fl As New file_list
    Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' width='500' height='500' frameborder='0' src='' scrolling='no'></iframe>"))
%> 
<form id='frmx' name='frmx' method="post"></form>
<div class="c1">
        <div id="resignx" onclick="javascript:showHideSubMenu(this,'divresign');" style="height:42px; width:300px;color:blue; font-size:14px;">
        <span style='width:80px; float:left; position:relative;'>Resign</span>
        <span style="float:left;width:75px; position:relative; background: url(images/gif/collapsed.gif) no-repeat;">&nbsp;</span></div>
    
    <div id="divresign" style='display:none;'>
        <asp:Literal ID="outp" runat="server"></asp:Literal>
        
        
    </div>
    <div id="div1" style='display:none;'>
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        
        
    </div>
</div>
<div class='sp'>&nbsp;</div><%'column space
                                %>
 <div class='c2'>
 <div id="Div2" onclick="javascript:showHideSubMenu(this,'div3');" style="height:42px; width:auto; color:blue; font-size:16px;">
         <span style='width:80px; float:left; position:relative;'>Reports</span>
        <span style="float:left;width:75px; position:relative; background: url(images/gif/collapsed.gif) no-repeat;">&nbsp;</span></div>
   
    <div id="div3" style='display:none;'>
        <asp:Literal ID="otp2" runat="server"></asp:Literal>
        
        
    </div>
    
    </div>
 <div class='sp'>&nbsp;</div><div class='c3'>
  <div id="Div4" onclick="javascript:showHideSubMenu(this,'div7');" style="height:42px; width:auto; color:blue; font-size:16px;">
         <span style='width:80px; float:left; position:relative;'>Reports Increament</span>
        <span style="float:left;width:75px; position:relative; background: url(images/gif/collapsed.gif) no-repeat;">&nbsp;</span></div>

         <div id="div7" style='display:none;'>
 <asp:Literal ID="outp3" runat="server"></asp:Literal></div></div>

  
    
   
    
</body>
</html>
