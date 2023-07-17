<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pardimreg.aspx.vb" Inherits="pardimregx" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%
    dim namelist as string =""
    Dim fm As New formMaker
    Dim ds As New dbclass
    Dim sec As New k_security
    Session.Timeout = "60"
    Dim arrv(4) As String
    Dim check As String = ""
    Dim cach As String = ""
    Dim i As Integer = 0
    Dim msg As String = ""
    Dim projid As String
    Dim spl() As String
    Dim t1, t2 As Date
    Dim pdate1, pdate2 As Date
    pdate1 = "#1/1/1900#"
    pdate2 = "#1/1/1900#"

   Dim projname As String
    If String.IsNullOrEmpty(Session("pprroojj")) Then
        
        Session("pprroojj") = ""
        If Request.Form("projname") <> "" Then
            If Request.Form("projname") <> Session("pprroojj") And Request.Form("month") & "/1/" & Request.Form("year") <> Session("whn") Then
                Session("pprroojj") = Request.Form("projname")
                Session("whn") = Request.Form("month") & "/1/" & Request.Form("year")
                pdate1 = Session("whn")
                pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
           
            End If
        End If
        
    Else
        If Request.QueryString("sub") = "" Then
            
            If Request.Form("projname") <> Session("pprroojj") And Request.QueryString("task") = "" Then
                If Request.Form("projname") <> "" Then
                    Session("pprroojj") = Request.Form("projname")
                    Session("whn") = Request.Form("month") & "/1/" & Request.Form("year")
                    pdate1 = Session("whn")
                    pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
                End If
            ElseIf Request.Form("month") <> "" Then
                If Request.Form("month") & "/1/" & Request.Form("year") <> Session("whn") Then
                    Session("whn") = Request.Form("month") & "/1/" & Request.Form("year")
                    pdate1 = Session("whn")
                    pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
                End If
            Else
                
                pdate1 = Session("whn")
                pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
    
            End If
        Else
            ' Response.Write(Request.QueryString("sub"))
            Session("pprroojj") = ""
            Session("whn") = "#1/1/0001#"
        End If
    End If
    If IsError(Session("whn")) = False And Session("whn") <> "" Then
        ' Session("pprroojj") = ""
        ' Session("whn") = "#1/1/0001#"
        pdate1 = Session("whn")
        pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
    
        
    End If
    %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head><title></title>

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/script.js" type="text/javascript"></script>
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">

<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script src="jqq/ui/jquery.ui.progressbar.js"></script>
	<script src="jqq/ui/jquery.ui.datepicker.js"></script>
<script src="jq/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
	<script type="text/javascript" src="scripts/form.js"></script>
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/time.js" type="text/javascript"></script>
<style type="text/css" enableviewstate="true">
#listatt
{
    font-size:9pt;
    border: 1px blue solid;
    text-align:left;
}

</style>
	<%  		    Dim ptype As String
	    namelist = fm.getjavalist2("tblproject", "project_name,project_id", session("con"), "|")
	    '  ptype = fm.getjavalist2("pardimpay", "distinct payment_typ", Session("con"), ",")
	    ' Response.Write(ptype)
 %>
 <script type="text/javascript">
 var year="<% response.write(request.form("year")) %>";
 var month="<% response.write((cint(request.form("month"))-1).tostring) %>";
 $("#st").text(window.status);
 
 function checkall()
 {
    if($(".chkbox").is(':checked'))
    {
        $("#chkall").text("Checked All");
        
        $(".chkbox").attr("checked",false);
    }
    else
    {
        $(".chkbox").attr("checked",true);
       $("#chkall").text("Un-checked All");
    }
 
 }
 //alert(document.referrer.toString())

 function findid(idx)
 {

 var idArr = [];
 var val=[];
 var output="";

var trs = document.getElementsByTagName("td");

for(var i=0;i<trs.length;i++)
{
   var arrid=[]; 
  
   
   arrid=trs[i].id.split("_")
   if(arrid.length>1)
   {
        obj="#paid-" + arrid[0];
     // alert($(obj).is(':checked'));
        if($(obj).is(':checked')&& $(obj).attr('disabled')!="disabled")
    {
        idArr.push(trs[i].id);
       
        
    }
        
        
   }
  
}
    for(var i=0;i<idArr.length;i++){
     val.push($("#" + idArr[i]).text());
         //alert(val[i]);
   if(val[i]!=="")
   output +=idArr[i]+"="+val[i]+"&";
   }
     //  alert("koooookuuuuluuuu" +idArr[i]+"===="+ $(obj).val());
          var str2=$("#frmpay").formSerialize();
       var str=$("#frmpay").fieldValue();
       //foreach(document.getElementById)
 // alert(output+str2+str);
   // var loct="<%response.write(session("emp_id")) %>/leave/" +fn+"/";
   if(output!==""){
  var hw= window.screen.availHeight;
  var ww=window.screen.availWidth;
  //$("#pay").css({width:'900px',height:'500px'});
  
   // alert(whr + fn.toString());
   if($("#reason").val()!=="" && $("#form_date").val()!=="" && $("#to_date").val()!=="" && output!==""){
     if($("#paiddate").val()!="" && $("#mthd").val()=="")
     {
        alert("paid Method is not selected");
        $("#mthd").focus();
     }
     else if($("#paiddate").val()=="" && $("#mthd").val()!="")
     {
         alert("paid paid-Date is not Enter");
     $("#paiddate").focus();
     }
     else
     {
     
       $("#fpay").attr("frameborder","0");
       $("#hidpass").val(output);
       $('#frmpay').attr("action","?save=on&savebd="+idx+"&"+str2);
       $('#frmpay').submit();
      }
     }     
    }
  }

 </script>
 <script type="text/javascript" language="javascript">
 //dateafter('to_date','paiddate');
 //dateafter('from_date','to_date');
 
    function print(loc,title,head,footer)
    {
    
    var printFriendly = document.getElementById(loc)
    var printWin = window.open("print.htm",title,"menubar=no,status=no,toolbar=no,width=900px,scrollbars=yes,height=400px,top=20px,left=20px");
    printWin.document.write("<html><head><meta http-equiv='content-type' content='application/winword' /><meta name='content-disposition' content='attachment; filename=payrol.doc' /><title>" + head + "</title></head><body>" + printFriendly.innerHTML + "<label class='smalllbl'>" + footer + "</label></body></html>");
    printWin.document.close();
   printWin.window.print();   
  // printWin.close();
    }
    <%
  if Request.QueryString("month")<>"" then
response.write("var month=" & chr(34) & request.querystring("month") & chr(34) & ";")
response.write("var year=" & chr(34) & request.querystring("year") & chr(34) & ";")
response.write("var projname=" & chr(34) & sec.dbStrToHex(request.querystring("projname")) & chr(34) & ";")

else
response.write("var month=" & chr(34) & request.form("month") & chr(34) & ";")
response.write("var year=" & chr(34) & request.form("year") & chr(34) & ";")
response.write("var projname=" & chr(34) & sec.dbStrToHex(request.form("projname")) & chr(34) & ";")

end if 

%>

  
 var paymth="<% response.write(request.querystring("paymth")) %>";
 //alert(document.referrer.toString())
 var ppname="<% response.write(sec.dbStrToHex(request.querystring("projname"))) %>";
 
   
  
 var nameproj=[<% response.write(namelist) %>];
 
    $(document).ready(function() {
     $( "#projname" ).autocomplete({
  source: function(req, response) { 
    var re = $.ui.autocomplete.escapeRegex(req.term); 
    var matcher = new RegExp( "^" + re, "i" ); 
    response($.grep(  nameproj, function(item){ 
        return matcher.test(item); }) ); 
        }
		});
		});

  <% 
 
  if session("pprroojj")<>"" then
    spl=session("pprroojj").split("|")
    if spl.length>1 then
        projname=spl(0)
        projid=spl(1)
        Dim listinproj As String = fm.getprojemp(projid, pdate2, Session("con"))
        dim sql as string
       ' response.write("//" & session("pprroojj"))
       sql=" emp_static_info as esi inner join emprec on esi.emp_id=emprec.emp_id " & _
         "where emprec.id " & _
         "in" & _
         "(" & listinproj & ")"
         namelist = fm.getjavalist2(sql, " esi.first_name,esi.middle_name,esi.last_name", session("con"), " ")
     response.write("/*" & sql & "*/")
    else
    end if
    end if
    if namelist<>"" then
    
%>
var namelist=[<% response.write(namelist) %>];
<% end if %>
function checkdate(){
var d1;
var d2;
d1=$( '#to_date' ).text();
d2=$("from_date").val();
alert(document.getElementById("to_date").value());
alert(d2);
getdate(d1,"mm/dd/yy");
if(d1-d2>0)
{   
    alert("Date value is not correct");
    $("#to-date").val("");
}
}
    </script>
</head>
<body>
<div style="width:100%; background:#6879aa; text-align:center;color:White; font-size:19pt;"> 
    Payment Registration<form name="frmlistout" action="pardimreg.aspx" method="post" style="font-size:12pt;">
       
        <!-- Payment type:<input type="text" name='ptype' id='ptype' /-->
       
         Project Name: <input type="text" name="projname" id="projname" />
               <select id="month" name="month">
            <%  For i = 1 To 12
                    Response.Write("<option value='" & i.ToString & "'>" & MonthName(i) & "</option>")
                Next
                
                %>
        </select>
        <script type="text/javascript">
            $("#month").val("<% response.write(today.month) %>");
        </script>
         <select id="year" name="year">
            <%  For i = Today.Year To Today.Year - 9 Step -1
                    Response.Write("<option value='" & i.ToString & "'>" & i.ToString & "</option>")
                Next%>
        </select>
        Increment/Decrement in Selected month: <input type="checkbox" value="thismonth" id="thism" name="thism" />
         <input type="submit" value="submit" />
    </form>
    </div>
    <span onclick="javascript:disp('viewg');" style="color:Blue; cursor:pointer">show per-diem</span>
     <div id='viewg' style="font-size:8pt; overflow:auto;">
      <form id="form1" runat="server">
    <div style="font-size:6pt;">
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            CellPadding="4" ForeColor="#333333" GridLines="None" Font-Size="9pt">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ItemStyle-Font-Size="8pt" />
            </Columns>
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ></asp:SqlDataSource>
    </div>
    </form></div>
    <div id="printform">
    <style type="text/css">
        #tb1
        {
            border:1px black solid;
        }
         #tb1 td
        {
            border-right:1px black solid;
            border-bottom:1px black solid;
        }
    </style>
    
    <form id="frmpay" name="frmpay" method="post" action="">
    <input type="hidden" name="hidpass" id="hidpass" />
    <%  'Response.Write(msg)
        If Request.Form("projname") <> "" Then
           
            If Request.Form("thism") <> "" Then
                Session("projnow") = outpy()
                Response.Write(Session("projnow"))
            Else
                %>
                
       <input type="hidden" name="projname" id="projn" value="<% response.write(request.Form("projname")) %>" />
               <select id="Select1" name="month" style="visibility:hidden;">
            <%  For i = 1 To 12
                    Response.Write("<option value='" & i.ToString & "'>" & MonthName(i) & "</option>")
                Next
                
                %>
        </select>
        <script type="text/javascript">
            $("#Select1").val("<% response.write(request.Form("month")) %>");
        </script>
         <select id="Select2" name="year" style="visibility:hidden;">
            <%  For i = Today.Year To Today.Year - 9 Step -1
                    Response.Write("<option value='" & i.ToString & "'>" & i.ToString & "</option>")
                Next%>
        </select>
        <script type="text/javascript">
            $("#Select2").val("<% response.write(request.Form("year")) %>");
        </script>
                <%
                    'Response.Write(Request.Form("projname"))
                    Session("projnow") = outpx()
                     Response.Write(Session("projnow"))
                    ' Response.Write(outpx)
                   
                End If
           
                'outp &= "</form></div>"
            Else
                If IsPostBack = True Then
                    Response.Write(Session("projnow"))
                End If
            End If
                
        datamanip()
     %></form></div>
     
    <%  If Session("command") = "Edit" Then
            
           
                %>
             <script type="text/javascript">
            $("#viewg").css('height','350px');
            $("#viewg").css("overflow","none");
        </script>
            <% 
        End If
       
        %>
       
</body>
</html>
