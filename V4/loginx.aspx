<%@ Page Language="VB" AutoEventWireup="false" CodeFile="loginx.aspx.vb" Inherits="loginx" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.io" %>

<%@ Import Namespace="system.data" %>

<%@ Import Namespace="system.data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<% 
   
    ' Response.Write("popen")%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>Untitled Page</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" /> 
    <link href="css/style.css" rel="stylesheet" type="text/css"/>
        <link href="css/layout.css" rel="stylesheet" type="text/css"/>
	<link href="css/message.css" rel="stylesheet" type="text/css"/>
	        <script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>

	<script type="text/javascript" src="scripts/style.js"></script>
	<script src="scripts/script.js" type="text/javascript"></script>
	<script type="text/javascript" src="scripts/archive.js"></script>
    <script type="text/javascript" src="scripts/jquery/jquery.js"></script>
    <script type="text/javascript" src="scripts/jquery/jquery.validate.js"></script>
    <script type="text/javascript" src="scripts/jquery/jquery.form.js"></script>
    <script type="text/javascript" src="scripts/jquery/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="js/validate.js"></script>
   
    <script type="text/javascript" src="js/core.js"></script>
  
    
    <link rel="stylesheet" type="text/css" media="screen" href="css/main.css" />
<link rel="stylesheet" type="text/css" media="screen" href="css/module.report.css" />
<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" />
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script language=javascript>
    function msgoutxx(titlex, msgtype, msg) {

        $("#fpay").attr("frameborder", "0");
        $('#frmx').attr("target", "fpay");
        $('#frmx').attr("action", "msgbox.aspx?msgtitle=" + titlex + "&msgtype=" + msgtype + "&msg=" + msg);

        //$('#frmx').attr("action", "rptbankpayrol.aspx" + val + "&month=" + month + "&year=" + year + "&projname=" + projname);
        $('#frmx').submit();
        showonly("payx");
        $("#pay").dialog({
            top: 30,
            title: 'Bank St.',
            height: 600,
            width: 850,
            modal: true
        });

       
    }
</script>

</head>
<body>
  <%  
      Dim msg As String = ""
      msg = checklogin(Request.Form("txtUsername"), Request.Form("txtPassword"))
      Dim nf As New formMaker
      'Response.Write(nf.form(session("con"), "emp_static_info"))
      Dim fl As New file_list
      Dim l() As String
     ' Response.Write(fl.dialog("payx", "", "<iframe name='fpay' id='fpay' width='500' height='500' frameborder='0' src='' scrolling='no'></iframe>"))
      Response.Write(fl.msgboxt("payx", Request.QueryString("titlex"), "<iframe name='fpay' id='fpay' width='900' height='500' frameborder='0' src='' scrolling='yes'></iframe>"))
      l = File.ReadAllLines("C:\temp\backup\passs.kst")

      For i As Integer = 0 To l.Length - 1
          'Response.Write(l(i) & "<br>")
       
          If i = 1 Then
              Session("mailpass") = l(i)
             
              ' Response.Write(amt & "====" & px(2) & " ===" & px(1) & "<br>")

              Exit For
          End If
      Next
      
      %>
      <div id='outer' style='width:100%; height:100%; '>
      <div style='left:0px; top:0px; position:relative;'><%Response.Write(Application("hour") & " " & Application("count"))%></div>
      <form id='frmx' name='frmx' method='post'></form>
      <div id='pay' style="display:none"></div>
<div id="divLogin">
    <div class="divLogo" align="center">
    <span style="font-size:36px; text-align:justify; font-style:italic; text-align:center; width:100%; color:#4499b1;">
   <% response.write(session("company_name")) %> </span><br /><span style="color:#4499b1;"><% Response.Write(Application("baddress"))%></span></div>
<div class="logcont" style="display:inline">
    <form id="frmLogin" method="post" action="">
               <div id="logInPanelHeading">LOGIN Panel
</div>  <div id="divUsername" class="textInputContainer">
<span id="log_position" class="textInputContainer"></span>
         <input name="txtUsername" id="txtUsername" type="text" />          
         <span class="form-hint" >Username</span> 
        </div>
        <div id="divPassword" class="textInputContainer">
            <input name="txtPassword" id="txtPassword" type="password" />         
            <span class="form-hint" >Password</span>
        </div>
        <div id="divLoginHelpLink"></div>
        <div id="divLoginButton">
            <input type="submit" name="Submit" class="button" id="btnLogin" value="LOGIN" />
                    </div>
    </form></div>

</div>


   
<div id="divFooter" align="center">
    <span id="spanCopyright">
    <asp:Label ID="ccx" runat="server"></asp:Label>
        
    </span>
    <span id="spanSocialMedia">
        
    </span>
    <br class="clear" />
</div>
 <% 'Response.Write("contacted")
     Session("con").close()%>
<script type="text/javascript">
    
    function calculateUserTimeZoneOffset() {
        var myDate = new Date();
        var offset = (-1) * myDate.getTimezoneOffset() / 60;
        return offset;
    }
            
    function addHint(inputObject, hintImageURL) {
        if (inputObject.val() == '') {
            inputObject.css('background', "url('" + hintImageURL + "') no-repeat 10px 3px");
        }
    }
            
    function removeHint() {
       $('.form-hint').css('display', 'none');
    }
    
    function showMessage(message) {
        if ($('#spanMessage').size() == 0) {
            $('<span id="spanMessage"></span>').insertAfter('#btnLogin');
        }

        $('#spanMessage').html(message);
    }
    
    function validateLogin() {
        var isEmptyPasswordAllowed = false;
        
        if ($('#txtUsername').val() == '') {
            showMessage('Username cannot be empty');
             $('#txtUsername').focus();
            return false;
        }
        
        if (!isEmptyPasswordAllowed) {
            if ($('#txtPassword').val() == '') {
                showMessage('Password cannot be empty');
                 $('#txtPassword').focus();
                return false;
            }
        }
        return true;
    }

    $(document).ready(function () {
        /*Set a delay to compatible with chrome browser*/
        $("#txtUsername").focus();
        //$(document).keyup(function(){ removeHint();});
        setTimeout(checkSavedUsernames, 100);
        $('#txtUsername').keyup(function () {
            removeHint();
        });
        $('#txtUsername').focus(function () {
            removeHint();
        });

        $('#txtPassword').focus(function () {
            removeHint();
        });

        $('.form-hint').click(function () {
            removeHint();
            $('#txtUsername').focus();
        });

        $('#hdnUserTimeZoneOffset').val(calculateUserTimeZoneOffset().toString());

        $('#frmLogin').submit(validateLogin);
        <% if request.QueryString("titlex")<>"" then %>
        msgoutxx('<% response.write(request.querystring("titlex")) %>', '<% response.write(request.querystring("msgtype")) %>', '<% response.write(request.querystring("msg"))%>');
        <%end if %>
    });

    function checkSavedUsernames(){
        if ($('#txtUsername').val() != '') {
            removeHint();
        }
    }

    if (window.top.location.href != location.href) {
        window.top.location.href = location.href;
    }
</script>
    	<script type="text/javascript">
   	showobjar("frmLogin","log_position",470,130);
	</script>    
	<% 		   	    	    ' MsgBox(msg)
	    Response.Write(msg)
	    If msg <> "" Then
	        %>
	        <script language="javascript" type="text/javascript">
	        showMessage('<% response.write(msg) %>');
	        </script>
	        <% 	            msg = ""
	        End If
	        Response.Write(Request.QueryString("msg"))
	        %>
    </div> 
   
      </body>
  
</html>
