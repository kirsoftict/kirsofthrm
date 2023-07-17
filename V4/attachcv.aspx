<%@ Page Language="VB" AutoEventWireup="false" CodeFile="attachcv.aspx.vb" Inherits="attachcv" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.IO" %>
<%   If Session("username") = "" Then
       %>
       <script type="text/javascript">
 //document.location.href="admin_home.php"
window.location="logout.aspx";
</script>
       <%
       End If%>
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

	<script type="text/javascript" src="jq/ui/jquery.ui.resizable.js"></script>
	<!--script type="text/javascript" src="jq/ui/jquery.ui.button.js"></script--><script type="text/javascript" src="jq/ui/jquery.ui.dialog.js"></script>
	<link rel="stylesheet" href="jq/demos.css" />
	
        
</head>
<body style="height:400px; overflow:auto;">
<% Dim fm As New formMaker
    
    'Response.Write(fm.helpback(False, True, False, False, "", ""))%>
<%  Dim msg As String = ""
    Dim up As New file_list
    Dim paths, loc As String
    paths = ""
    loc = ""
    loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
    
    If Session("emp_id") = "" Then%>
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

    <%  
        If Request.QueryString("upload") = "on" Then
           
            
               
            msg = Session("path")
            Dim arr() As String = {".doc", ".pdf", ".docx"}
            'Response.Write(Server.MapPath("employee") & "\" & Session("emp_id") & "\cv\")
            msg = up.fupload(Request.Files("pic"), Server.MapPath("employee") & "\" & Session("emp_id") & "\cv", "1024000", arr)
            If msg = "upload complete" Then
              
                'Dim sql As String = "update emp_static_info set imglink='employee/" & Session("emp_id") & "/" & "cv/" & _
                ' up.findfilename(Request.Files("pic").FileName) & "' where emp_id='" & Session("emp_id") & "'"
                'Dim db As New dbclass
                'db.save(sql, session("con"),session("path"))
                'db = Nothing
                
            End If
                %>
        <script type="text/javascript">
        
   // $("#form1").attr('target',"_self");
   //$("#form1").attr("action","attachcv.aspx");
	//       $("#form1").delay(800000000);
	//         $("#form1").submit();


        </script>
        <%
        End If
       
    End If
   
   %>
    <form id="form1" method="post" action="" enctype="multipart/form-data">
    <div id="messagebox"></div><div>
    <% 'Response.Write(Session("emp_id"))%>
   Attach Resume *: <input type="file" name="pic" id="pic" />
    </div>
    <div><label class="lblsmall">Please Upload MS Word(*.doc;*.docx) or PDF file Max size limit 1MB 
</label><label id="msgbox"></label></div>
    <div><!--input type="button" id="btnUpload" value="Upload" /--></div>
    </form>
    <br /><br />
     <script type="text/javascript">
      function del(val,ans)
       { 
        if( ans=="1st")
          { // alert(val);
                $('#frmx').attr("target","_self");
                $('#frmx').attr("action","<% response.write(loc) %>?task=delete&id="+val+"&tbl=file");
                $('#frmx').submit();
            }
        else
        {
       // str=$("#frmemp_static_info").formSerialize();
        //alert("deleted");
         //  $("#messagebox").load("deletefile.aspx?fname=" + val + "&tasks=delete");

              $('#frmx').attr("target","_self");
             $('#frmx').attr("action","<% response.write(loc) %>?task=deletecon&id="+val+"&delete=true");
             $('#frmx').submit();
        }
            
       }
         </script>
         <div style=" height:200px;">
        <div style="height:24px; width:100%; background-color:Blue;color:White; font-size:15px;">Uploaded files</div>
    
    <% 
       
        If Request.QueryString("id") <> "" Then
            paths = Request.QueryString("id")
            paths = paths.Replace("~", "\")
        End If
        If Request.QueryString("delete") = "true" Then
           
            'Response.Write(paths)
            msg = up.deletefile(paths)
        End If
        If Session("emp_id") <> "" Then
        %>
         <%  Dim p As String
         p = Server.MapPath("employee") & "\" & Session("emp_id") & "\cv\"
             If Directory.Exists(p) = False Then
                 
                 up.makedir(p)
             End If
             If Directory.Exists(p) = True Then
                 Dim root As String
                 Dim ext As String = ""
                 Dim fname As String
                 root = "employee/" & Session("emp_id").trim & "/cv/"
                 For Each k As String In Directory.GetFiles(p)
                %>
               <div style="display:inline; float:left; width:90px;">
                <span style=" display:block">
                
            <% 
                Select Case up.file_ext(k).ToLower
                    Case ".doc", ".docx"
                        fname = "msword"
                    Case ".pdf"
                        fname = "pdf_icon"
                    Case Else
                        fname = "unknown"
                End Select
                Dim ff As String
                ff = k.Replace("\", "~")
                
              
                %>
            <img src="images/gif/<%response.write(fname) %>.gif" height="40px" width="40px" alt="  <% response.write(up.findfilename(k)) %>" title="  <% response.write(up.findfilename(k)) %>" />
            <br />
            <span><%  Dim fn As String = up.findfilename(k)
                      If fn.Length > 8 Then
                          fn = fn.Substring(0, 5) & "~." & up.file_ext(k)
                      End If
                      Response.Write(fn)%></span><br />
            <span><a href="javascript:del('<%response.write(ff) %>','1st');">delete</a></span>&nbsp;&nbsp;&nbsp;<span><a href="<%response.write(root & up.findfilename(k))  %>">View</a></span>
        </span>
        </div>
        <div style="width:15px; float:left;">&nbsp;</div>
               <%
               Next
           End If
        
            
          
            
           
            %>
    
    </div>
    <% end if %>
   
    <form id="frmx" action="" method="post">
    </form>
    <%  
        If Request.QueryString("task") = "delete" Then
            '  Response.Write(Request.QueryString("id"))
            Dim fs As New file_list
            Dim con As String
            con = "<span style='color:red;'> This file (" & up.findfilename(paths) & ") will be deleted, <br />Are you sure you want delete it?<br /><hr>" & _
           "<img src='images/gif/btn_delete.gif' style='cursor:pointer;' onclick=" & Chr(34) & "javascript:del('" & Request.QueryString("id") & "','yes');" & Chr(34) & _
           "></span>"
            'fs.msgboxt("del123", "Caution! Deleting", con)
      %> <div id="dialog-modal" title="Caution">
      <img src="images/alert.gif" width="20px" height="20px" alt="" style=" float:left;" /><% response.write(con) %></div>
           <script type="text/javascript">
          
           //$( "#dialog:ui-dialog" ).dialog( "destroy" );
	
		$( "#dialog-modal" ).dialog({
		resizable: true,
			modal: true
	
		});
           </script>
           <%
        End If
     %>
</body>
</html>

<script type="text/javascript">
$(document).ready(function(){
   $("#messagebox").text("<% Response.Write(msg) %>"); 

});
	    $('#pic').change(function()
	    {
	        if(checktype(this.value,'docx,.doc,.pdf'))
	        {   $("#form1").attr("action","attachcv.aspx?upload=on");
	         showMessage("","msgbox");
	         //alert(document.form1.value);
	         $("#form1").submit();
	            }
	      
	     else
	     {
	        $("#messagebox").text("");
	        showMessage("Sorry File type is not Supported","msgbox");
	        }
	    });
</script>
