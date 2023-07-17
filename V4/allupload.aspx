<%@ Page Language="VB" AutoEventWireup="false" CodeFile="allupload.aspx.vb" Inherits="allupload" %>

<%@ Import Namespace="kirsoft.hrm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>
<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.core.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript"  src="jq/ui/jquery.ui.mouse.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.draggable.js"></script>
	<script type="text/javascript" src="scripts/form.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.datepicker.js"></script>
	<link rel="stylesheet" href="jq/demos.css" />
	
</head>
<body>
 <% If Session("username") = "" Then
       %>
       <script type="text/javascript">
 //document.location.href="admin_home.php"
window.location="logout.aspx";
</script>
       <%
          
       End If
      
       If Session("emp_id") = "" Then
       %>

<script type="text/javascript">
 //document.location.href="admin_home.php"
//window.location="empcontener.aspx";
</script>

<%
Else
     %>

<script type="text/javascript">
//alert('<% response.write(session("emp_id")) %>');
</script>

<%    
End If %>
<form id='frmx' name='frmx' method="post" action=""></form>
    <form id="form1" method="post" action="" enctype="multipart/form-data">
    <div>
    <% 'Response.Write(Session("emp_id"))%>
    File: <input type="file" name="pic" id="pic" />
    </div>
    <div><label class="lblsmall">Max. size <%  Dim size As Integer = 0
                                               size = Request.QueryString("size")
                                               Response.Write((size / 1000 / 1000).ToString & "MB")
     %>. file type <%  Dim type As String = Request.QueryString("ftype")
                       Dim loct As String = Request.QueryString("loct")
                       Response.Write(type)%> only</label><label id="msgbox"></label></div>
    <div><input type="button" id="btnUpload" value="Upload" /></div>
    </form>
    <% 
    If Session("emp_id") = "" Then
        %>
        <script type="text/javascript">
        
    $("#form1").attr('target',"_parent");
   $("#form1").attr("action","emplist.aspx");
	        
	         $("#form1").submit();


        </script>
        <%
            'Response.Redirect("listemp.aspx")
        Else
        %>

    <script type="text/javascript">
            var emp="yes";
    </script>

    <%  Dim msg As String = ""
        If Request.QueryString("upload") = "on" Then
            Dim up As New file_list
            msg = Session("path")
            Dim arr() As String = type.Split(",")
            'Response.Write(Server.MapPath("employee") & "\" & Request.QueryString("loct"))
            msg = up.fupload(Request.Files("pic"), Server.MapPath("employee") & "\" & loct, size, arr)
            If msg = "upload complete" Then
                'Dim sql As String = "update emp_static_info set imglink='employee/" & Session("emp_id") & "/" & "idpic/" & _
                ' up.findfilename(Request.Files("pic").FileName) & "' where emp_id='" & Session("emp_id") & "'"
                'Dim db As New dbclass
                'db.save(sql, session("con"),session("path"))
                ' db = Nothing
                Response.Write(msg)
                If Request.QueryString("loc") <> "" And Request.QueryString("tar") <> "" Then%>
        <script type="text/javascript">
 $("#frmx").attr('target',"<% response.write(Request.QueryString("tar")) %>");
$("#frmx").attr("action","<% response.write(Request.QueryString("loc")) %>");
	//     '  $("#form1").delay(50000);
	 $("#frmx").submit();
 showMessage("<% response.write(msg) %>","msgbox");

        </script>
        <%End If
    End If
                %>
        <script type="text/javascript">
  // '$("#form1").attr('target',"_parent");
  //'$("#form1").attr("action","empcontener.aspx");
	//     '  $("#form1").delay(50000);
	//       '  $("#form1").submit();
 showMessage("<% response.write(msg) %>","msgbox");

        </script>
        <%
        End If
        Response.Write(msg)
        End If
        
    
        %>
</body>
</html>

<script type="text/javascript">
$(document).ready(function(){
    

});
	    $('#pic').change(function()
	    {
           // alert(this.value+'<% response.write(request.querystring("ftype")) %>');
	        if(checktype(this.value,'<% response.write(request.querystring("ftype")) %>'))
	        {   $("#form1").attr("action","allupload.aspx?upload=on&ftype=<%response.write(type) %>&size=<%response.write(size) %>&loct=<%response.write(loct) %>&loc=<% response.write(request.querystring("loc")) %>&tar=<% response.write(request.querystring("tar")) %>");
	         showMessage("","msgbox");
	         $("#form1").submit();
	            }
	     else
	        showMessage("Sorry File type is not Supported","msgbox");
	    });
</script>