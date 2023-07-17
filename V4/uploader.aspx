<%@ Page Language="VB" AutoEventWireup="false" CodeFile="uploader.aspx.vb" Inherits="uploader" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<% 
    If Session("con").State = ConnectionState.Closed Then
        Response.Write("closed")
        Session("con").open()
    End If
    ' Dim dtm As New datematter
   

   %> 
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>


<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />

	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
	
	
	

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
	
	<script type="text/javascript" src="scripts/form.js"></script>
	
	<script type="text/javascript" src="jqq/ui/jquery.ui.datepicker.js"></script>
<!--script src="jq/jquery-ui-timepicker-addon.js" type="text/javascript"></script-->


  <script  type="text/javascript" src="jqq/ui/jquery.ui.button.js"></script>
  <script  type="text/javascript" src="jqq/ui/jquery.ui.dialog.js"></script>
      <link rel="stylesheet" type="text/css" media="screen" href="css/print600.css" /> 
  
  <%  
     
'Response.Write(Session("fullempname"))

Dim sc As New k_security
' Response.Write(sc.d_encryption("zewde@123"))
If Request.Form.HasKeys = True Then
    'Dim db As New dbclass
    ' Dim sql As String
    ' sql = db.makest("tblworkexp", Request.Form, session("con"), "")
    'Response.Write(sql)
    'db.excutes2(sql, Session("con"), Session("path"), Session("emp_iid"))
End If
For Each p As String In Request.Form
    'Response.Write(" <br />" & p & "=>" & Request.Form(p))
 
Next
For Each k As String In Request.ServerVariables
    ' Response.Write("<br />" & k & "=>" & Request.ServerVariables(k))
Next
'Response.Write("<br />" & Request.Form("do"))

 %>
 <script type="text/javascript">
 

		
	function   subupload(xxx)
    {
        if($("#"+xxx).val()!=="")
          $("#uploadatt").submit();

    }	
  

 </script>
</head>
<%  Dim filename, locx As String
    locx = Server.MapPath("")
    Dim fsx As New file_list
  
    
    Dim root As String = locx
   ' Response.Write(Request.Files("xlxfile").FileName)
    Try
        
       
        locx &= "\salaryup"
        root = sc.dbStrToHex(locx)
        If Request.Files("xlxfile").FileName <> "" Then
            
           
           
            locx &= "\" & Request.Form("updatesel")
            If Directory.Exists(locx) = False Then
                Directory.CreateDirectory(locx)
            End If
            Response.Write(locx & "<br>........")
            Dim obj As Object
            obj = Request.Files("xlxfile")
       
            Dim ftype() As String = {".xls"}
          
            filename = sc.StrToHex3(Format(Now, "Mddyyhhmmss"))
            '  Response.Write(filename & "<br>...............ffffffffffxxxxxxxxxxx" & Request.Form("updatesel"))
            ' Response.Write(sc.HexToStr3(filename))
            ' filename = sc.Kir_StrToHex(filename)
            Dim size As Double = 10000000
            ' Response.Write(locx & "iiiiiiiiiiiiiin")
       
            Dim rtn As String
            rtn = fsx.fupload(obj, locx, size, ftype, filename)
            Response.Write(rtn)
    
        End If
    Catch ex As Exception
        Response.Write(ex.ToString)
    End Try
    %>
<body style="height:auto;">
<% ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))%>
 
<div id='tim' style='left:0px;top:0px; height:20px; position:inherit;'></div>


 
 
 <div style="float:left; padding:0px 0px 0px 20px;width:500px;">
    <form enctype="multipart/form-data" method="post" name="uploadatt" id="uploadatt">
            <select id='updatesel' name='updatesel'>
            <option value='bones'>Bones and Increment</option>
            <option value='salinc'>Salary Increment</option>
            </select>
        upload Excel file: <input type="file" id="xlxfile" name="xlxfile" accept="application/vnd.ms-excel" /><label style="color:Gray;font-size:9px;">only .xls file</label>
        <button name="upload" value="upload" onclick="subupload('xlxfile')">Upload</button>
    </form>
    <div>
       <%
         
           %> 
    </div>
 </div>
    <div>
       <%
         '  locx &= "\bones"
           Dim fld() As String
           Dim fls() As String
           Dim sec As New k_security
           Dim fm As New formMaker
           Dim svf() As String = {""}
           fld = Directory.GetDirectories(locx)
           If File.Exists(locx & "/dist.txt") = True Then
               svf = File.ReadAllLines(locx & "/dist.txt")
           End If
           fls = Directory.GetFiles(locx)
           Dim vrtn As String = "<table>"
           For k As Integer = 0 To fld.Length - 1
               fls = Directory.GetFiles(fld(k))
               For j As Integer = 0 To fls.Length - 1
            
                   If fm.searcharray(svf, fls(j)) = False Then
                       vrtn &= "<tr><td> " & fsx.findfilename(fls(j)) & "</td><td><a href='?dist=on&fp=" & sec.dbStrToHex(fls(j)) & "'>distribut</a></td></tr>"
                   End If
             
               
               Next
           Next
           vrtn &= "</table>"
           Response.Write(vrtn)
          
           Dim listx As String
           If Request.QueryString("dist") = "on" Then
      
               listx = incdist(Request.QueryString("fp"))
              %> 
           
         
           <% 
               Response.Write(listx)
             
       End If
      %>
    </div>
    <div style="clear:left;">&nbsp;</div>
 
<div id="newpage">
<%
    %>
<iframe src="filesystem.aspx?root=<%=root %>" width='100%' height='400'></iframe>
</div>
<script type="text/javascript" language="javascript">
    hform = findh(document.getElementById("middle_bar"));
    $('.titlet').text("Increament reg.");
    //showobjar("formx","titlet",22,2);
   
  </script>
  
   
    <form id="frmx" action="" method="post">
    </form>
   
   
   
   
</body>
</html>


