<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pardimform2n.aspx.vb" Inherits="pardimform2n" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%
    dim namelist as string =""
    Dim fm As New formMaker
    dim ds as new dbclass
    Session.Timeout = "60"
    Dim spl2() As String
    Dim arrv(4) As String
    Dim check As String = ""
    Dim cach As String = ""
    Dim sql() As String
    Dim i As Integer = 0
    Dim msg As String = ""
    If Request.QueryString("save") = "on" Then
        Dim df As Integer
        Dim d1, d2 As Date
        d1 = Request.QueryString("from_date")
        d2 = Request.QueryString("to_date")
        df = d2.Subtract(d1).Days + 1
        For Each p As String In Request.QueryString
            
            spl2 = p.Split("_")
            If spl2.Length > 1 Then
                
                If IsNumeric(spl2(0)) = True Then
                    If cach <> spl2(0) Then
                        cach = spl2(0)
                        ReDim Preserve sql(i + 1)
                        check = fm.getinfo2("select id from pardimpay where emptid=" & spl2(0).ToString & " and from_date='" & Request.QueryString("from_date") & "' and to_date ='" & Request.QueryString("to_date") & "'", Session("con"))
                        If check = "None" Then
                            sql(i) = "insert into pardimpay(emptid,pardim,reason,no_days,from_date,to_date,who_reg,date_reg) values("
                            sql(i) &= spl2(0).ToString & "," & Request.QueryString(spl2(0) & "_" & 3) & ",'" & Request.QueryString("reason") & "',"
                            sql(i) &= df.ToString & ",'" & d1 & "','" & d2 & "','" & Request.QueryString("who_reg") & "','" & Request.QueryString("date_reg") & "')"
                            
                            i += 1
                        End If
                    
                    End If
                    
                    
                End If
                'Response.Write(spl2(0) & "<br>")
                
            End If
            ' Response.Write(p & "==" & Request.QueryString(p) & "<br>")
        Next
        Dim flg As String = ""
        If sql.Length > 0 Then
            ds.save("begin", Session("con"), Session("path"))
            
            For i = 0 To UBound(sql)
                flg = ds.save(sql(i), Session("con"), Session("path"))
                'Response.Write(sql(i) & "<br>")
                If flg <> "1" Then
                    ds.save("rollback", Session("con"), Session("path"))
                    flg = "rollback"
                    Exit For
                    
                End If
            Next
            If flg <> "rollback" Then
                flg = ds.save("commit", Session("con"), Session("path"))
                
                If flg <> "-1" Then
                    msg = "Datas Not Saved Please Contact the Admin."
                Else
                    msg = "Data is Saved"
                End If
            Else
                msg = flg & " Data is not Saved, may it existed"
            End If
        End If
        If check = "None" Then
            msg = "Data already exist"
        End If
    End If
    %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head><title></title>
<script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.core.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript"  src="jq/ui/jquery.ui.mouse.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.draggable.js"></script>
	<script type="text/javascript" src="scripts/form.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.resizable.js"></script>
	<!--script type="text/javascript" src="jq/ui/jquery.ui.button.js"></script-->
	<script type="text/javascript" src="jq/ui/jquery.ui.dialog.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.datepicker.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.autocomplete.js"></script>
<script src="jq/jquery-ui-timepicker-addon.js" type="text/javascript"></script>

<style type="text/css" enableviewstate="true">
#listatt
{
    font-size:9pt;
    border: 1px blue solid;
    text-align:left;
}

</style>
	<%  	   
	    namelist = fm.getjavalist2("tblproject", "project_name,project_id", session("con"), "|")

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
function view(frm)
{
    var spl=frm.split("-");
    $('#'+frm).attr("action","pardimview.aspx?s=" + spl[1]);
       
      $('#' +frm).submit();
   // alert(spl[1]);
}
 function findid(frm)
 {
    
 var idArr = [];
 var val=[];
 var output="";
 var i;
var trs = document.getElementsByTagName("input");

for(i=0;i<trs.length;i++)
{
   var arrid=[]; 
  
   
   arrid= trs[i].id.split("-")
   if(arrid.length>1)
   {
        
        obj="#" + trs[i].id;
       // alert($(obj).attr('disabled'));
        if($(obj).is(':checked')&& $(obj).attr('disabled')!="disabled")
    {
        idArr.push(trs[i].id);
        
    }
        
        
   }
  
}
    for(i=0;i<idArr.length;i++){
     
    output +=i+"="+idArr[i]+"&";
   }
     //  alert("koooookuuuuluuuu" +idArr[i]+"===="+ $(obj).val());
          var str2=$("#" +frm).formSerialize();
          //alert(str2)
     //  var str=$("#" +frm).fieldValue();
       //foreach(document.getElementById)
 //alert(output+str2+str);
   // var loct="<%response.write(session("emp_id")) %>/leave/" +fn+"/";
   if(output!==""){
  var hw= window.screen.availHeight;
  var ww=window.screen.availWidth;
  //$("#pay").css({width:'900px',height:'500px'});
  
   // alert(whr + fn.toString());
  
   
    // $('#'+frm).attr("target","fpay");
     // $("#"+frm).attr("frameborder","0");
   // output="perdiemsheet.htm?" + str2;
     //    var printWinx = window.open("perdiemsheet.aspx","Per-diem","menubar=no,status=yes,toolbar=no,width=900px,scrollbars=yes,height=400px,top=20px,left=20px");
if(confirm("Data is going to save, are you sure")){
      $('#'+frm).attr("action","perdiemsheet.aspx?save=on&"+output+str2);
       
      $('#' +frm).submit();
       }
      // $('#pay').css({top:'0px',left:'0px'});
      // showobj("pay");
       }
  // $("#nextpage").val(output);
  // $("#frmpay").attr("action","?" );
  //$("#frmpay").submit();
 
 }
 var namelist=[<% response.write(namelist) %>];
    $(document).ready(function() {
     $( "#projname" ).autocomplete({
  source: function(req, response) { 
    var re = $.ui.autocomplete.escapeRegex(req.term); 
    var matcher = new RegExp( "^" + re, "i" ); 
    response($.grep(  namelist, function(item){ 
        return matcher.test(item); }) ); 
        }
		});
		});
 </script>
 <script type="text/javascript" language="javascript">
    function print(loc,title,head,footer)
    {
    
    var printFriendly = document.getElementById(loc)
    var printWin = window.open("print.htm",title,"menubar=no,status=no,toolbar=no,width=900px,scrollbars=yes,height=400px,top=20px,left=20px");
    printWin.document.write("<html><head><meta http-equiv='content-type' content='application/winword' /><meta name='content-disposition' content='attachment; filename=payrol.doc' /><title>" + head + "</title></head><body>" + printFriendly.innerHTML + "<label class='smalllbl'>" + footer + "</label></body></html>");
    printWin.document.close();
   printWin.window.print();   
  // printWin.close();
    }
   
    </script>
</head>
<body>
<div style="width:100%; height:45px; background:#6879aa; text-align:center;color:White; font-size:19pt;">Perdiem Payment Form</div>
    <form name="frmlistout" action="" method="post">
       
         Type Project Name:<input type="text" name="projname" id="projname" />
       Not Paid<input type="checkbox" name="paidst" id="paidst" value="no" />
         <input type="submit" value="submit" />
    </form>
    <%  Dim rs As DataTableReader
        Dim emptid As String
        If Request.Form("projname") <> "" Then
            'Response.Write(Request.Form("projname"))
            Dim spl(), outp As String
            Dim projid As String = ""
           
            spl = Request.Form("projname").Split("|")
            If spl.Length <= 1 Then
                ReDim spl(2)
                spl(0) = Request.Form("projname")
                spl(1) = ""
            End If
            outp = ""
            projid = spl(1)
            If Request.Form("paidst") = "no" Then
                rs = ds.dtmake("selectpar", "select * from pardimpay where emptid in(select emptid from emp_job_assign where project_id='" & projid & "') and paid_state='n' order by emptid", Session("con"))
                Dim copy As String = ""
                Dim header As String = ""
                Dim color As String = ""
                Dim amt As Double = 0
                If rs.HasRows = True Then
                    header = ""
                    header = "<table id='tb1' cellspacing='0' cellpadding='0' width='700px'>"
                    While rs.Read
                        emptid = rs.Item("emptid")
                        If copy <> emptid Then
                            If color = "#aabbbf" Then
                                color = "#C8DBF0"
                                ' header &= "<tr><td colspan='4' style='text-align:center;'>Paid</td></tr></table>"
                            Else
                           
                                color = "#aabbbf"
                                '  
                            End If
                            If header <> "<table id='tb1' cellspacing='0' cellpadding='0' width='700px'>" Then
                                'Response.Write("innnner")
                                header &= "<tr>" & _
                                           "<td colspan='5'><select name='mthd-" & emptid & "' id='mthd-" & emptid & "'>" & _
                                           "<option value='Cheque'>Cheque</option>" & _
                                           "<option value='Bank'>Cheque</option>" & _
                                            "<option value='Cash'>Cash</option></select>" & _
                                            "<input type='text' id='paiddate-" & emptid & "' name='paiddate-" & emptid & "' value='" & Today.Month & "/" & Today.Day & "/" & Today.Year & "'></td>" & _
                                            "<script language='javascript' type='text/javascript'> $(function() {$( '#paiddate-" & emptid & "').datepicker({changeMonth: true,changeYear: true	}); $( '#paiddate-" & emptid & "' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>" & _
                                            "<td colspan='' style='text-align:center;cursor:pointer; color:red;' onclick=" & Chr(34) & _
                                            "javascript:findid('form-" & emptid & "');" & Chr(34) & _
                                            " >Paid</td></tr></table></form></td></tr>"
                            End If
                            ' header &= "<tr><td colspan='4' style='text-align:center;'>Paid</td></tr></table>"
                            header &= "<tr style='background:" & color & ";'><td colspan='8'><table width='500px' align='left'><tr><td>Name</td><td>:</td><td>" & _
                            fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con")), Session("con")).ToString & _
                            "</td><td>Emp. Id</td><td>:</td><td>" & emptid & _
                            "</td></tr></table></td></tr><tr style='background:" & color & _
                            ";font-weight:bold;'><td colspan='7' style='padding-left:20px;'>" & _
                            "<form id='form-" & emptid & "' name='form-" & emptid & _
                            "' method='post' action=''><table style='border:1px solid black;'><tr><td>Reason</td><td>Date Depart<span style='font-size:8pt; color:gray;'>(MM/DD/YYYY)</span></td><td>Date Return<span style='font-size:8pt; color:gray;'>(MM/DD/YYYY)</span></td><td>No. Days</td><td>Per diem</td><td>Amount</td></tr>"
                            copy = emptid
                        End If
                        amt = CDbl(rs.Item("no_days")) * CDbl(rs.Item("pardim"))
                        header &= "<tr style='background:" & color & ";'><td>" & _
                        rs.Item("reason") & "</td><td>" & rs.Item("from_date") & "</td><td>" & _
                        rs.Item("to_date") & "</td><td>" & rs.Item("no_days") & _
                        "</td><td style='text-align:right'>" & fm.numdigit(rs.Item("pardim"), 2).ToString & _
                        "</td><td style='text-align:right;'>" & fm.numdigit(amt, 2).ToString & _
                        "</td><td><input type='checkbox' name='paid-" & emptid & "-" & _
                        rs.Item("id") & "' id='paid-" & emptid & "-" & rs.Item("id") & "' value='" & _
                        emptid & "-" & rs.Item("id") & "'/></td></tr>"
                     
                    
                    End While
                    header &= "<tr>" & _
                   "<td colspan='5'><select name='mthd-" & emptid & "' id='mthd-" & emptid & "'>" & _
                   "<option value='Cheque'>Cheque</option>" & _
                   "<option value='Bank'>Cheque</option>" & _
                    "<option value='Cash'>Cash</option></select>" & _
                    "<input type='text' id='paiddate-" & emptid & "' name='paiddate-" & emptid & "' value='" & Today.Month & _
                    "/" & Today.Day & "/" & Today.Year & "'></td>" & _
                    "<script language='javascript' type='text/javascript'> $(function() {$( '#paiddate-" & emptid & "').datepicker({changeMonth: true,changeYear: true	}); $( '#paiddate-" & emptid & "' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>" & _
                    "<td colspan='' style='text-align:center;cursor:pointer; color:red;' onclick=" & Chr(34) & _
                    "javascript:findid('form-" & emptid & "');" & Chr(34) & _
                    " >Paid</td></tr></table></form></table>"
                    header = "<div><style> #tb1  { border:1px solid black;} #tb1 td{ border-bottom:1px solid black; border-right:1px solid black;}</style>" & header & "</div>"
                    Response.Write(header)
                End If
            Else
                rs = ds.dtmake("selectpar", "select * from pardimpay where emptid in(select emptid from emp_job_assign where project_id='" & projid & "') and paid_state='y' order by ref", Session("con"))
                Dim copy As String = ""
                Dim header As String = ""
                Dim color As String = ""
                Dim amt As Double = 0
                Dim refc As String = ""
                If rs.HasRows = True Then
                    header = ""
                    header = "<table id='tb1' cellspacing='0' cellpadding='0' width='700px'>"
                    While rs.Read
                        refc = rs.Item("ref")
                        emptid = rs.Item("emptid")
                        If copy <> refc Then
                            'Response.Write(refc & "<br>")
                            If color = "#aabbbf" Then
                                color = "#C8DBF0"
                                ' header &= "<tr><td colspan='4' style='text-align:center;'>Paid</td></tr></table>"
                            Else
                           
                                color = "#aabbbf"
                                '  
                            End If
                            If header <> "<table id='tb1' cellspacing='0' cellpadding='0' width='700px'>" Then
                                ' Response.Write("innnner")
                                header &= "<tr><td colspan='6' style='text-align:center;cursor:pointer; color:red;' onclick=" & Chr(34) & "javascript:view('view-" & copy & "');" & Chr(34) & " >View</td></tr></table></form></td></tr>"
                            End If
                            ' header &= "<tr><td colspan='4' style='text-align:center;'>Paid</td></tr></table>"
                            header &= "<tr style='background:" & color & ";'><td colspan='8'><table width='500px' align='left'><tr><td>Name</td><td>:</td><td>" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con")), Session("con")).ToString & _
                            "</td><td>Emp. Id</td><td>:</td><td>" & emptid & "</td></tr></table></td></tr><tr style='background:" & color & ";font-weight:bold;'><td colspan='7' style='padding-left:20px;'>" & _
                            "<form id='view-" & refc & "' name='view-" & refc & "' method='post' action=''><table style='border:1px solid black;'><tr><td>Reason</td><td>Date Depart<span style='font-size:8pt; color:gray;'>(MM/DD/YYYY)</span></td><td>Date Return<span style='font-size:8pt; color:gray;'>(MM/DD/YYYY)</span></td><td>No. Days</td><td>Per diem</td><td>Amount</td></tr>"
                            copy = refc
                        End If
                        amt = CDbl(rs.Item("no_days")) * CDbl(rs.Item("pardim"))
                        header &= "<tr style='background:" & color & ";'><td>" & rs.Item("reason") & _
                        "</td><td>" & rs.Item("from_date") & "</td><td>" & rs.Item("to_date") & "</td><td>" & _
                        rs.Item("no_days") & "</td><td style='text-align:right'>" & _
                        fm.numdigit(rs.Item("pardim"), 2).ToString & "</td>" & _
                        "<td style='text-align:right;'>" & fm.numdigit(amt, 2).ToString & _
                        "</td><td></td></tr>"
                     
                    
                    End While
                    header &= "<tr><td colspan='6' style='text-align:center; cursor:pointer; color:red;' onclick=" & _
                     Chr(34) & "javascript:view('view-" & refc & "');" & Chr(34) & " >view</td></tr></table></form></table>"
                    header = "<div><style> #tb1  { border:1px solid black;} #tb1 td{ border-bottom:1px solid black; border-right:1px solid black;}</style>" & header & "</div>"
                    Response.Write(header)
                End If
            End If
        End If
        
        
     %>
</body>
</html>
