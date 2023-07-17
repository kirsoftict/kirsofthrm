<%@ Page Language="VB" AutoEventWireup="false" CodeFile="uploadpic.aspx.vb" Inherits="uploadpic" %>

<%@ Import Namespace="kirsoft.hrm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
	
<script type="text/javascript" src="jq/ui/jquery.ui.datepicker.js"></script>
	<link rel="stylesheet" href="jq/demos.css" />
	<script type="text/javascript" src="scripts/form.js"></script>
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
window.location="empcontener.aspx";
</script>

<%
Else
     %>

<script type="text/javascript">
//alert('<% response.write(session("emp_id")) %>');
</script>

<%    
End If %>
    <form id="form1" method="post" action="" enctype="multipart/form-data">
    <div>
    <% 'Response.Write(Session("emp_id"))%>
    File: <input type="file" name="pic" id="pic" />
    </div>
    <div><label class="lblsmall">Max. size 1M. file type jpg,gif,png only</label><label id="msgbox"></label></div>
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
            Dim arr() As String = {".jpg", ".gif", ".png"}
            'Response.Write(Server.MapPath("employee") & "\" & Session("emp_id") & "\idpic\")
            msg = up.fupload(Request.Files("pic"), Server.MapPath("employee") & "\" & Session("emp_id") & "\idpic", "1024000", arr)
            If msg = "upload complete" Then
                Dim sql As String = "update emp_static_info set imglink='employee/" & Session("emp_id") & "/" & "idpic/" & _
                up.findfilename(Request.Files("pic").FileName) & "' where emp_id='" & Session("emp_id") & "'"
                Dim db As New dbclass
                db.save(sql, session("con"),session("path"))
                db = Nothing
            End If
                %>
        <script type="text/javascript">
 //$("#form1").attr('target',"frm_tar");
   //$("#form1").attr("action","empcontener.aspx?emp_id=<% response.write(session("emp_id")) %>");
	//      $("#form1").delay(50000);
	//     $("#form1").submit();


        </script>
        <%
        End If
        Response.Write("<span style='color:red'>" & msg & "</span>")
        End If
        
    
        %>
</body>
</html>

<script type="text/javascript">
$(document).ready(function(){
    

});
	    $('#pic').change(function()
	    {
	        if(checktype(this.value,'.jpg,.png,.gif'))
	        {   $("#form1").attr("action","uploadpic.aspx?upload=on");
	         showMessage("","msgbox");
	         $("#form1").submit();
	            }
	     else
	        showMessage("Sorry File type is not Supported","msgbox");
	    });
</script>
