<%@ Page Language="VB" AutoEventWireup="false" CodeFile="filesystem.aspx.vb" Inherits="filesystem" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
 <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>
<link rel="stylesheet" type="text/css" media="print, handheld" href="css/media.css"/>
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css"/>
	<script type="text/javascript" src="jqq/jquery-1.9.1.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.core.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.menu.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.dialog.js"></script>
<script type="text/javascript" src="jqq/ui/jquery.ui.button.js"></script>
    <script type="text/javascript" src="jqq/ui/jquery.ui.datepicker.js"></script>
<script type="text/javascript" src="scripts/form.js"></script>

<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
  
	<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" />

    <script language='javascript'>

        function clicked(path, type) {
            $("#pathx").val(path);
            $("#clickedtype").val(type);
            $("#frmpath").attr("action", "filesystem.aspx");
            $("#frmpath").submit();

        }
        function createfolder() {
           
            if ($("#foldername").val() == "") {
                
                $('#addnew').css({ top: '0px', left: '0px' });
                $("#addnew").remove("display");
                $("#addnew").dialog({
                    height: 300,
                    width: 600,
                    modal: true
                });
            }
            else if ($("#foldername").val() !== "") {
                $("#frmaddnew").attr("action", "filesystem.aspx?createfolder=on");
                $("#frmaddnew").submit();
            }

        }
        function uploadfile() {

            if ($("#fileup").val() == "") {

                $('#upload').css({ top: '0px', left: '0px' });
                $("#upload").remove("display");
                $("#upload").dialog({
                    height: 300,
                    width: 600,
                    modal: true
                });
            }
            else if ($("#fileup").val() !== "") {
                $("#frmupload").attr("action", "filesystem.aspx?upload=on");
                $("#frmupload").submit();
            }

        }
        function deletef(path) {
            $("#fpath").val(path);
            $("#frmpath").attr("action", "filesystem.aspx?del=on");
            $("#frmpath").submit();
        }
        function download(path) {
           // alert('download.aspx?down=' + path);
            // $("#msgout").text("download.aspx?down=" + path);
            $('#frmpay').attr("target", "_blank");
                        $('#frmpay').attr("action", 'download.aspx?down=' + path);
            $('#frmpay').submit();
            
           
        }

        function delfld(path) {
            $("#fpath").val(path);
            $("#frmpath").attr("action", "filesystem.aspx?delfld=on");
            $("#frmpath").submit();
        }

        function backfld() {
            
            $("#frmpath").attr("action", "filesystem.aspx?return=on");
            $("#frmpath").submit();
        }
    </script>
</head>
<body>
 <form id='frmpay' name='frmpay' method="post" action=""></form>
    <form id='frmpath' name='frmpath' method="post" action="">
    <input type="hidden" name="pathx" id="pathx" />
    <input type="hidden" name="fpath" id="fpath" />
    <input type="hidden" name='clickedtype' id='clickedtype' />
    </form>
    <%  If Session("username") <> "" Then
        Else
           Response.Write("<script>window.location='logout.aspx?msg=session time out';</script>")
        End If
        Dim sec As New k_security
        Dim fl As New file_list
        If Request.QueryString("del") = "on" Then
            deletefile(sec.dbHexToStr(Request.Form("fpath")))
           
        ElseIf Request.QueryString("createfolder") = "on" Then
            createdir(Request.Form("foldername"))
        ElseIf Request.QueryString("upload") = "on" Then
            If Request.Files("fileup").FileName <> "" Then
                Dim arr() As String = {""}
                Dim arrnot() As String = {".exe", ".bat"}
               Response.Write( fupload(Request.Files("fileup"),Session("folder"),2048000,arr,arrnot))
            End If
            
        ElseIf Request.QueryString("delfld") = "on" Then
            deldir(sec.dbHexToStr(Request.Form("fpath")))
        Else
            If Request.ServerVariables("QUERY_STRING") = "" Then
                
            End If
              
        End If
        
        If Request.QueryString("return") = "on" Then
            Dim spl() As String
            
            If Session("folder") <> "" Then
                Session("folder").replace("/", "\")
                spl = Session("folder").split("\")
                '  Response.Write(spl.Length)
                Session("folder") = ""
                For i As Integer = 0 To spl.Length - 2
                    '   Response.Write("<br>" & spl(i))
                    Session("folder") &= spl(i) & "\"
                Next
               
                Session("folder") = Session("folder").substring(0, Session("folder").length - 1)
                ' Response.Write(Session("folder"))
            End If
        Else
            'Response.Write(Session("folder"))
            
        End If
        
        %>
    <style>
        #outer
        {
        	width:1000px;
        	min-height:600px;
        	background:#fafafa;
        	border:1px solid gray;
       
        	}
        #outer	#inner
        	    {
        	    	width:1000px;
        	    	padding:1px 1px 1px 1px;
        	    	min-height:600px;
        	    	border:1px soldi black;
        	    	
        	    	}
        	    #inner	#upper
        	    	{
        	    	    width:auto;
        	    	    height:7%;
        	    		color:red;
        	    		margin: 2px 2px 2px 2px;
        	    		}
        	    		#upper .address
        	    		{
        	    			width:auto;
        	    			background-color:White;
        	    			}
        	    			#upper .cmdline
        	    			{
        	    				background-color:Gray;
        	    				}
        	    		#inner #down
        	    				{
        	    					width:auto;
        	    					height:auto;
        	    					}
        	    			#down .downleft
        	    			{
        	    			    height:500px;
        	    			   	float:left;
        	    				width:26%;
        	    				border:1px solid gray;
        	    				
        	    				overflow:auto;
        	    				}
        	    				#down .spacer
        	    			{
        	    				float:left;
        	    				width:1%;
        	    				
        	    				background:#ffaacc;
        	    				}
        	    				#down .downright
        	    			{
        	    			    height:500px;
        	    				float:left;
        	    				width:72%;
        	    				border:1px solid gray;
        	    				overflow:auto;
        	    				}
        	    				#outer a
        	    				{
        	    					text-decoration:none;
        	    					color:Black;
        	    					
        	    					}
    </style>


    <%
    
        Try
            '  Response.Write("<br>>>>>>>>>>>>>>>>>>>>>>>>" & Session("folder") & "<<<<<<<<<<<<<<br>")
            If Session("folder") = "" Then
                Session("folder") = Server.MapPath("")
            End If
            If Request.Form("pathx") <> "" Then
                Session("folder") = sec.dbHexToStr(Request.Form("pathx"))
                ' Session("orgfld") = sec.dbHexToStr(Request.Form("pathx"))
            ElseIf Request.QueryString("root") <> "" Then
                Session("folder") = sec.dbHexToStr(Request.QueryString("root"))
                Session("orgfld") = sec.dbHexToStr(Request.QueryString("root"))
                Session("orgfld") = Session("orgfld").replace("/", "\")
            ElseIf Session("folder").ToString.Length < Server.MapPath("").ToString.Length Then
                Session("folder") = Server.MapPath("")
                
            Else
               ' Session("folder") = Server.MapPath("")
            End If
            'Response.Write(Session("folder") & "<<<<<<<<<<<<<<")
            %>
    <div id='outer'>
    <span id="msgout"></span>
        <div id='inner'>
            <div id='upper'>
           
            <div class='cmdline'><span>
            <%  If address(Session("folder")) = "\" Then
                      %>  <img src="images/filesystem/Back-482.png" style="width:32px; height:32px;"/><%
                Else
                   %>  <img src="images/filesystem/Back-48.png"  style="cursor:pointer; width:32px; height:32px;" onclick="javascript:backfld();" /><%
                End If%>
           </span>    
          <span class='cmdline'><%=Address(Session("folder"))%></span> 
          <span style='float:right'>
          <img src="images\filesystem\blue/upload.jpg" style="width:32px; height:32px; cursor:pointer;" alt="Upload"  onclick='javascript:uploadfile();' /></span>  
          <span style='float:right'>
          <img src="images\filesystem\blue/adddir.png" style="width:32px; height:32px; cursor:pointer;" alt="creatFolder"  onclick='javascript:createfolder();' /></span>  
           </div>
            </div>
            <div id='down'>
                <div class='downleft'> <% =folders(Session("folder")) %></div><div class='spacer'></div>
                <div class='downright'><% = files(Session("folder"))%></div>
            </div>
            <div><%sortdir(Session("path"))%></div>
        </div>
    
   
    <%
    Catch ex As Exception
    Response.Write(ex.ToString & "<br>" & Request.Form("pathx") & "<br>" & Session("folder"))
    End Try%>
    <div id='addnew' style='display:none'>
    <form name='frmaddnew' id='frmaddnew' method="post" enctype="multipart/form-data">
      Folder Name:  <input type="text" id='foldername' name='foldername'  />
      <input type="button" id='btncreate' value="create" onclick='javascript:createfolder()' />
    </form>
    </div>
    <div id='upload' style='display:none'>
    <form name='frmupload' id='frmupload' method="post" enctype="multipart/form-data">
      Folder Name:  <input type="file" id='fileup' name='fileup'  />
      <br /><input type="checkbox" name='replace' value="on" />Replace<br />
      <input type="button" id='Button1' value="upload" onclick='javascript:uploadfile();' />
    </form>
    </div>
</body>
</html>
