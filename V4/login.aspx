<%@ Page Language="VB" AutoEventWireup="false" CodeFile="login.aspx.vb" Inherits="login" %>
<%@ Import Namespace = "system.data"%>
<%@ import Namespace="system.data.sqlclient" %>
<%@ import Namespace="system.IO" %>
<%@ import Namespace="Kirsoft.hrm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<% 
   
    Session("cm_auth") = "24"
    logcopy()
    ' Response.Write(Format(Now, "M/dd/yyyy HH:mm:ss"))
    ' Response.Write("<br>" & Application("count"))
    ' Response.Write("popen")%>
<html class="no-js" lang="en">

<head>
    <meta charset="utf-8">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
    <title>Login | <%= Session("company_name")%></title>
    <meta name="description" content="">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- favicon
		============================================ -->
    <link rel="shortcut icon" type="image/x-icon" href="images/netlog.png">
    <!-- Google Fonts
		============================================ -->
    <link href="https://fonts.googleapis.com/css?family=Play:400,700" rel="stylesheet">
    <!-- Bootstrap CSS
		============================================ -->
    <link rel="stylesheet" href="netsystem/css/bootstrap.min.css">
    <!-- Bootstrap CSS
		============================================ -->
    <link rel="stylesheet" href="netsystem/css/font-awesome.min.css">
    <!-- owl.carousel CSS
		============================================ -->
    <link rel="stylesheet" href="netsystem/css/owl.carousel.css">
    <link rel="stylesheet" href="netsystem/css/owl.theme.css">
    <link rel="stylesheet" href="netsystem/css/owl.transitions.css">
    <!-- animate CSS
		============================================ -->
    <link rel="stylesheet" href="netsystem/css/animate.css">
    <!-- normalize CSS
		============================================ -->
    <link rel="stylesheet" href="netsystem/css/normalize.css">
    <!-- main CSS
		============================================ -->
    <link rel="stylesheet" href="netsystem/css/main.css">
    <!-- morrisjs CSS
		============================================ -->
    <link rel="stylesheet" href="netsystem/css/morrisjs/morris.css">
    <!-- mCustomScrollbar CSS
		============================================ -->
    <link rel="stylesheet" href="netsystem/css/scrollbar/jquery.mCustomScrollbar.min.css">
    <!-- metisMenu CSS
		============================================ -->
    <link rel="stylesheet" href="netsystem/css/metisMenu/metisMenu.min.css">
    <link rel="stylesheet" href="netsystem/css/metisMenu/metisMenu-vertical.css">
    <!-- calendar CSS
		============================================ -->
    <link rel="stylesheet" href="netsystem/css/calendar/fullcalendar.min.css">
    <link rel="stylesheet" href="netsystem/css/calendar/fullcalendar.print.min.css">
    <!-- forms CSS
		============================================ -->
    <link rel="stylesheet" href="netsystem/css/form/all-type-forms.css">
    <!-- style CSS
		============================================ -->
    <link rel="stylesheet" href="netsystem/style.css">
    <!-- responsive CSS
		============================================ -->
    <link rel="stylesheet" href="netsystem/css/responsive.css">
    <!-- modernizr JS
		============================================ -->
    <script src="netsystem/js/vendor/modernizr-2.8.3.min.js"></script>
    <script type="text/javascript">
        function msgoutxx(titlex, msgtype, msg) {

            $("#fpay").attr("frameborder", "0");
            $('#frmx').attr("target", "fpay");
            $('#frmx').attr("action", "msgbox.aspx?msgtitle=" + titlex + "&msgtype=" + msgtype + "&msg=" + msg);



            // $('#post').attr("disabled", "disabled");
            $('#payr').css({ top: '0px', left: '0px' });
            $("#payr").remove("display");
            $("#payr").dialog();

            $('#frmx').submit();
        }
        var hw, ww;
        hw = window.screen.availHeight;
        ww = window.screen.availWidth;
                   
  
  
    function showMessage(message) {
      
        $('.hlep').html(message);
    }
    
    

</script>
   <%  Dim msg As String
       If Request.Form("username") <> "" And Request.Form("password")<>"" Then
           msg = checklogin(Request.Item("username"), Request.Item("password"), Request.ServerVariables("URL"), Session("path"))
           
       End If
       Session("url") = Request.ServerVariables("HTTP_HOST")
      ' Response.Write(Session("url"))
        %>
</head>

<body>
    <!--[if lt IE 8]>
		<p class="browserupgrade">You are using an <strong>outdated</strong> browser. Please <a href="http://browsehappy.com/">upgrade your browser</a> to improve your experience.</p>
	<![endif]-->
	<div class="error-pagewrap">
		<div class="error-page-int">
			<div class="text-center m-b-md custom-login">
				<h3><img src='images\netlog.png' style='height:40px;'/> <%=session("company_name") %></h3>
				
			</div>
			<div class="content-error">
				<div class="hpanel">
                    <div class="panel-body">
                        <form action="" id="loginForm" method="post">
                            <div class="form-group">
                                <label class="control-label" for="username">Username</label>
                                <input type="text" placeholder="Username" title="Please enter you username" required="required" value="" name="username" id="username" class="form-control">
                                <span class="help-block small">Your unique username to app</span>
                            </div>
                            <div class="form-group">
                                <label class="control-label" for="password">Password</label>
                                <input type="password" title="Please enter your password" placeholder="******" required="required" value="" name="password" id="password" class="form-control">
                                <span class="help-block small">strong password</span>
                            </div>
                                    <% 
                                        
                                        If Request.QueryString("msg") <> "" Then
                                            msg = Request.QueryString("msg")%>
                              
                            <% End If
                                        
                                 If msg <> "" Then
                                    %>
                              <div  class="alert alert-danger alert-mg-b" role="alert">
                                
                +               <% Response.Write(msg)%>
                            </div>
                            <% End If%>

                            <button class="btn btn-success btn-block loginbtn">Login</button>
                           
                        </form>
                    </div>
                </div>
			</div>
			<div class="text-center login-footer">
				<% Response.Write("<a href='http://www.kirsoftet.com' target='_blank'>kirSoft</a> ver 1 &copy; kirSoft cp. " & Today.Year.ToString & " All rights reserved.")%>
			</div>
		</div>   
    </div>
    <!-- jquery
		============================================ -->
    <script src="js/vendor/jquery-1.12.4.min.js"></script>
    <!-- bootstrap JS
		============================================ -->
    <script src="js/bootstrap.min.js"></script>
    <!-- wow JS
		============================================ -->
    <script src="js/wow.min.js"></script>
    <!-- price-slider JS
		============================================ -->
    <script src="js/jquery-price-slider.js"></script>
    <!-- meanmenu JS
		============================================ -->
    <script src="js/jquery.meanmenu.js"></script>
    <!-- owl.carousel JS
		============================================ -->
    <script src="js/owl.carousel.min.js"></script>
    <!-- sticky JS
		============================================ -->
    <script src="js/jquery.sticky.js"></script>
    <!-- scrollUp JS
		============================================ -->
    <script src="js/jquery.scrollUp.min.js"></script>
    <!-- mCustomScrollbar JS
		============================================ -->
    <script src="js/scrollbar/jquery.mCustomScrollbar.concat.min.js"></script>
    <!--  <script src="js/scrollbar/mCustomScrollbar-active.js"></script>
   metisMenu JS
		============================================ -->
    <script src="js/metisMenu/metisMenu.min.js"></script>
    <script src="js/metisMenu/metisMenu-active.js"></script>
    <!-- tab JS
		============================================ -->
    <script src="js/tab.js"></script>
    <!-- icheck JS
		============================================ -->
    <script src="js/icheck/icheck.min.js"></script>
    <script src="js/icheck/icheck-active.js"></script>
    <!-- plugins JS
		============================================ -->
    <script src="js/plugins.js"></script>
    <!-- main JS
		============================================ -->
    <script src="js/main.js"></script>
    <!-- tawk chat JS
		============================================ -->
  
 
</body>

</html>

    
