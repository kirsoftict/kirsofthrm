<%@ Page Language="VB" AutoEventWireup="false" CodeFile="default3.aspx.vb" Inherits="Default3" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<html xmlns:v="urn:schemas-microsoft-com:vml"
xmlns:o="urn:schemas-microsoft-com:office:office"
xmlns:w="urn:schemas-microsoft-com:office:word"
xmlns:m="http://schemas.microsoft.com/office/2004/12/omml"
xmlns="http://www.w3.org/TR/REC-html40">
<head><meta http-equiv=Content-Type content="text/html; charset=utf-8"><title></title>
<style>
v\:* {behavior:url(#default#VML);}
o\:* {behavior:url(#default#VML);}
w\:* {behavior:url(#default#VML);}
.shape {behavior:url(#default#VML);}
</style>
<style>
@page
{
    mso-page-orientation: landscape;
    size:29.7cm 21cm;    margin:1cm 1cm 1cm 1cm;
}
@page Section1 {
    mso-header-margin:.5in;
    mso-footer-margin:.5in;
    mso-header: h1;
    mso-footer: f1;
    }
div.Section1 { page:Section1; }
table#hrdftrtbl
{
    margin:0in 0in 0in 900in;
    width:1px;
    height:1px;
    overflow:hidden;
}
p.MsoFooter, li.MsoFooter, div.MsoFooter
{
    margin:0in;
    margin-bottom:.0001pt;
    mso-pagination:widow-orphan;
    tab-stops:center 3.0in right 6.0in;
    font-size:12.0pt;
}
</style>
<xml>
<w:WordDocument>
<w:View>Print</w:View>
<w:Zoom>100</w:Zoom>
<w:DoNotOptimizeForBrowser/>
</w:WordDocument>
</xml>
</head>

<body>
<div class="Section1">

   <div class='page'><div class='subpage' >    <table width="100%" class='data table4' border="1"> <tbody><tr align="center">
            <td rowspan="2" width="35">
                
                    S/No
            </td>
            <td rowspan="2" width="150">
                <p align="center" >
                    <b>Name</b></p>
            </td>
            <td rowspan="2" width="110">
                <p align="center" >
                    <b>Position</b></p>
            </td>
<td rowspan="2" width="45">
                <p align="center" >
                    <b>Hire Date</b></p>
            </td>
            <td rowspan="2" width="22">
                <p align="center" >
                    <b>Sex</b></p>
            </td>
            <td rowspan="2" width="150">
                <p align="center" >
                    <b>Education Background</b></p>
            </td>
            <td rowspan="2" width="170">
                <p align="center" >
                    <b>Work place</b></p>
            </td>
            <td rowspan="2" width="71">
                <p align="center" >
                    <b>Emply. Status</b></p>
            </td>
            <td colspan="2"width="71">
                <p align="center" >
                    <b>Gross Pay</b></p>
            </td>
            <td rowspan="2" width="71">
                <p align="center" >
                    <b>Remark</b></p>
            </td>
        </tr>
         <tr style="line-height: 12px;" align="center">
            <td width="75">
                <p align="center" >
                    <b>Salary</b></p>
            </td>
            <td width="71">
                <p align="center" >
                    <b>Alw/Pr</b></p>
            </td>
        </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>1</span></td>
<td nowrap width='150' class='ename'><span>Yohannes Tademe Mergia</span></td>
<td nowrap width='110'  class='ename '> <span>Assistant Resident Eng </span> </td>
<td nowrap width='45' class='ename '><span>1/10/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Menebegna-Fincha-Shambu-Bako  Road Upgrading Project , Contract 1 : Menebegna-Fincha-Shambu' >Menebegna-Fincha-Shambu-Bako  Ro</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>18,550.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>2</span></td>
<td nowrap width='150' class='ename'><span>Ayalew Mulugeta Alemu</span></td>
<td nowrap width='110'  class='ename '> <span>Material Engineer </span> </td>
<td nowrap width='45' class='ename '><span>1/15/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Construction Technology'> <span>Construction Technology</span> </td>
  <td nowrap width='168' class='ename'><span title='Fiyel-Weha-Abi Adi Road Project, Contract 1: Fiyel Weha-Tekeze River ' >Fiyel-Weha-Abi Adi Road Project,</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>11,700.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>3</span></td>
<td nowrap width='150' class='ename'><span>Andualem Ketema Tadesse</span></td>
<td nowrap width='110'  class='ename '> <span>Works Inspector </span> </td>
<td nowrap width='45' class='ename '><span>1/17/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Bonga-Ameya - Chida Road Upgrading Project Contract 2: Felege Selam-Ameya-Chida' >Bonga-Ameya - Chida Road Upgradi</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>5,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>4</span></td>
<td nowrap width='150' class='ename'><span>Shewalul G/Silassie Gebre</span></td>
<td nowrap width='110'  class='ename '> <span>Senior Surveyor </span> </td>
<td nowrap width='45' class='ename '><span>1/18/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Surveying Technology'> <span>Surveying Technology</span> </td>
  <td nowrap width='168' class='ename'><span title='Arbaminch-Kemba-Sawla Contract 1 : Arbaminch-Belta Road Project' >Arbaminch-Kemba-Sawla Contract 1</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>5,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>5</span></td>
<td nowrap width='150' class='ename'><span>Daniel Ameneshewa Worneh</span></td>
<td nowrap width='110'  class='ename '> <span>Assistant Resident Eng </span> </td>
<td nowrap width='45' class='ename '><span>1/19/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Bahirdar-Zema Road Upgrading Project' >Bahirdar-Zema Road Upgrading Pro</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>16,730.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>6</span></td>
<td nowrap width='150' class='ename'><span>Giorgis Tilahun Berecha</span></td>
<td nowrap width='110'  class='ename '> <span>Structural Inspector </span> </td>
<td nowrap width='45' class='ename '><span>1/22/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Advanced Building Construction Technician'> <span>Advanced Building Constru</span> </td>
  <td nowrap width='168' class='ename'><span title='Bonga-Ameya - Chida Road Upgrading Project Contract 2: Felege Selam-Ameya-Chida' >Bonga-Ameya - Chida Road Upgradi</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>5,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>7</span></td>
<td nowrap width='150' class='ename'><span>Wosen W/Mariam W/Senbet</span></td>
<td nowrap width='110'  class='ename '> <span>Labratory Technician </span> </td>
<td nowrap width='45' class='ename '><span>1/23/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Construction Technology'> <span>Construction Technology</span> </td>
  <td nowrap width='168' class='ename'><span title='Awash-Mile Road  Overlay Project( Contract 1 & 2)' >Awash-Mile Road  Overlay Project</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>4,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>8</span></td>
<td nowrap width='150' class='ename'><span>Samson Fikremariam Gebremedhin</span></td>
<td nowrap width='110'  class='ename '> <span>Resident Engineer </span> </td>
<td nowrap width='45' class='ename '><span>1/23/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Teppi-Mizan Road Project' >Teppi-Mizan Road Project</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>20,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>9</span></td>
<td nowrap width='150' class='ename'><span>Mulay Tesfay Gebremedhen</span></td>
<td nowrap width='110'  class='ename '> <span>Works Inspector </span> </td>
<td nowrap width='45' class='ename '><span>1/23/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Road Construction'> <span>Road Construction</span> </td>
  <td nowrap width='168' class='ename'><span title='Fiyel-Weha-Abi Adi Road Project, Contract 1: Fiyel Weha-Tekeze River ' >Fiyel-Weha-Abi Adi Road Project,</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>4,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>10</span></td>
<td nowrap width='150' class='ename'><span>Gizachew Getie Kebtie</span></td>
<td nowrap width='110'  class='ename '> <span>Resident Engineer </span> </td>
<td nowrap width='45' class='ename '><span>1/25/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Bahirdar-Zegi (Kunzila Junction-Horticulture Farm-Zege Town Road Project)' >Bahirdar-Zegi (Kunzila Junction-</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>17,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>11</span></td>
<td nowrap width='150' class='ename'><span>Tadele Gidey Gebrekrstoss</span></td>
<td nowrap width='110'  class='ename '> <span>Material Inspector </span> </td>
<td nowrap width='45' class='ename '><span>1/25/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Building Construction Technology'> <span>Building Construction Tec</span> </td>
  <td nowrap width='168' class='ename'><span title='Fiyel-Weha-Abi Adi Road Project, Contract 1: Fiyel Weha-Tekeze River ' >Fiyel-Weha-Abi Adi Road Project,</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>4,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>12</span></td>
<td nowrap width='150' class='ename'><span>Terefe Girma Desta</span></td>
<td nowrap width='110'  class='ename '> <span>Hydrologist </span> </td>
<td nowrap width='45' class='ename '><span>2/1/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Hydraulic & Water Resources Engineering'> <span>Hydraulic & Water Resourc</span> </td>
  <td nowrap width='168' class='ename'><span title='Head Office Engineering Design Departement Employees' >Head Office Engineering Design D</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>20,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>3,000.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr></table><br><br> <table width='100%' class='sub_data table3' border='1'><tbody><tr>    <td width='306'>       <p>         <b><span>&nbsp;Prepared by ____________________</span></b></p> </td> <td width='286'>    <p>       <b><span>Approved By: _____________________</span></b></p> </td> <td width='62'>    <p>       &nbsp;</p> </td> </tr> <tr>    <td width='306'>       <p >          <b><span>&nbsp;Date _________________</span></b></p> </td> <td colspan='2' width='348'>    <p >       <b><span>Date ____________________</span></b></p> </td> </tr></tbody> </table> <br></div></div><div class='page'><div class='subpage' ><br><table width='100%' class='data' border='1'><tbody><tr><td>From:Jan 01, 2018-Jul 16, 2018</td><td> &nbsp;</td></tr><tr><td><p>Report type:Hired</p>
</td><td>
 <p class='MsoHeader'>
   <span>Page No.:</span></p>

 <p align='center' class='MsoHeader' style='text-align:center'>
 <span>Page </span><b><span>2</span></b><span> 
of </span><span><span><b>3</b></span></p></td></tr></tbody></table>  <table width="100%" class='data table4' border="1"> <tbody><tr align="center">
            <td rowspan="2" width="35">
                
                    S/No
            </td>
            <td rowspan="2" width="150">
                <p align="center" >
                    <b>Name</b></p>
            </td>
            <td rowspan="2" width="110">
                <p align="center" >
                    <b>Position</b></p>
            </td>
<td rowspan="2" width="45">
                <p align="center" >
                    <b>Hire Date</b></p>
            </td>
            <td rowspan="2" width="22">
                <p align="center" >
                    <b>Sex</b></p>
            </td>
            <td rowspan="2" width="150">
                <p align="center" >
                    <b>Education Background</b></p>
            </td>
            <td rowspan="2" width="170">
                <p align="center" >
                    <b>Work place</b></p>
            </td>
            <td rowspan="2" width="71">
                <p align="center" >
                    <b>Emply. Status</b></p>
            </td>
            <td colspan="2"width="71">
                <p align="center" >
                    <b>Gross Pay</b></p>
            </td>
            <td rowspan="2" width="71">
                <p align="center" >
                    <b>Remark</b></p>
            </td>
        </tr>
         <tr style="line-height: 12px;" align="center">
            <td width="75">
                <p align="center" >
                    <b>Salary</b></p>
            </td>
            <td width="71">
                <p align="center" >
                    <b>Alw/Pr</b></p>
            </td>
        </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>13</span></td>
<td nowrap width='150' class='ename'><span>Dawit Debalkie Haile</span></td>
<td nowrap width='110'  class='ename '> <span>Surveyor </span> </td>
<td nowrap width='45' class='ename '><span>2/1/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Advanced Surveying Technician'> <span>Advanced Surveying Techni</span> </td>
  <td nowrap width='168' class='ename'><span title='Adama-Awash Expressway Project' >Adama-Awash Expressway Project</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>5,600.00</span></td>
<td nowrap width='71' class='cssamt'><span>500.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>14</span></td>
<td nowrap width='150' class='ename'><span>Moges Berhe Nigusse</span></td>
<td nowrap width='110'  class='ename '> <span>Surveyor </span> </td>
<td nowrap width='45' class='ename '><span>2/1/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='-'> <span>-</span> </td>
  <td nowrap width='168' class='ename'><span title='Adama-Awash Expressway Project' >Adama-Awash Expressway Project</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>5,600.00</span></td>
<td nowrap width='71' class='cssamt'><span>500.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>15</span></td>
<td nowrap width='150' class='ename'><span>Abenezer Zegeye Sahle</span></td>
<td nowrap width='110'  class='ename '> <span>Surveyor </span> </td>
<td nowrap width='45' class='ename '><span>2/1/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Advanced Surveying Technician'> <span>Advanced Surveying Techni</span> </td>
  <td nowrap width='168' class='ename'><span title='Adama-Awash Expressway Project' >Adama-Awash Expressway Project</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>5,600.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>16</span></td>
<td nowrap width='150' class='ename'><span>Birhanu Wale Ferede</span></td>
<td nowrap width='110'  class='ename '> <span>Senior Labratory Techn </span> </td>
<td nowrap width='45' class='ename '><span>2/9/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Road Construction'> <span>Road Construction</span> </td>
  <td nowrap width='168' class='ename'><span title='Ditchot Galafi Junction-Elder-Belho-Design & Build Road Project' >Ditchot Galafi Junction-Elder-Be</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>8,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>1,500.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>17</span></td>
<td nowrap width='150' class='ename'><span>Roman Diriba Ethica</span></td>
<td nowrap width='110'  class='ename '> <span>Right of Way Managment </span> </td>
<td nowrap width='45' class='ename '><span>2/12/2018</span></td>
 <td nowrap width='22' class='ename '><span>Female</span></td>
  <td nowrap width='150' class='ename' title='Sociology and Social Work'> <span>Sociology and Social Work</span> </td>
  <td nowrap width='168' class='ename'><span title='Bahirdar-Zegi (Kunzila Junction-Horticulture Farm-Zege Town Road Project)' >Bahirdar-Zegi (Kunzila Junction-</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>7,054.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>18</span></td>
<td nowrap width='150' class='ename'><span>Hailu Dinkineh Gidabo</span></td>
<td nowrap width='110'  class='ename '> <span>Contract Engineer </span> </td>
<td nowrap width='45' class='ename '><span>2/14/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Construction Technology'> <span>Construction Technology</span> </td>
  <td nowrap width='168' class='ename'><span title='Head Office Construction Contract Administration & Supervision Departement Employees' >Head Office Construction Contrac</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>18,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>4,000.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>19</span></td>
<td nowrap width='150' class='ename'><span>Mesfin Kidane Belyneh</span></td>
<td nowrap width='110'  class='ename '> <span>Senior Surveyor </span> </td>
<td nowrap width='45' class='ename '><span>2/14/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Surveying Technology'> <span>Surveying Technology</span> </td>
  <td nowrap width='168' class='ename'><span title='Fiyel-Weha-Abi Adi Road Project, Contract 1: Fiyel Weha-Tekeze River ' >Fiyel-Weha-Abi Adi Road Project,</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>5,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>20</span></td>
<td nowrap width='150' class='ename'><span>Adanech Aschalew Tilahun</span></td>
<td nowrap width='110'  class='ename '> <span>Office attendant </span> </td>
<td nowrap width='45' class='ename '><span>2/19/2018</span></td>
 <td nowrap width='22' class='ename '><span>Female</span></td>
  <td nowrap width='150' class='ename' title='Road Construction'> <span>Road Construction</span> </td>
  <td nowrap width='168' class='ename'><span title='Head Office Adminstration & Finance Departement Employees' >Head Office Adminstration & Fina</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>1,540.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>21</span></td>
<td nowrap width='150' class='ename'><span>Endashaw Tsegaye Ambo</span></td>
<td nowrap width='110'  class='ename '> <span>Surveyor </span> </td>
<td nowrap width='45' class='ename '><span>2/19/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Surveying Technology'> <span>Surveying Technology</span> </td>
  <td nowrap width='168' class='ename'><span title='Mizan -Dima-Boma Upgrading Project Contract 1: Mizan-Dima(91.6)' >Mizan -Dima-Boma Upgrading Proje</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>4,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>22</span></td>
<td nowrap width='150' class='ename'><span>Segenet Tsehay Anteneh</span></td>
<td nowrap width='110'  class='ename '> <span>Secretery </span> </td>
<td nowrap width='45' class='ename '><span>2/20/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Secreterial Science & Office Management'> <span>Secreterial Science & Off</span> </td>
  <td nowrap width='168' class='ename'><span title='Bahirdar-Zegi (Kunzila Junction-Horticulture Farm-Zege Town Road Project)' >Bahirdar-Zegi (Kunzila Junction-</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>2,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>23</span></td>
<td nowrap width='150' class='ename'><span>Abebe Tesfaye Alemu</span></td>
<td nowrap width='110'  class='ename '> <span>Surveyor </span> </td>
<td nowrap width='45' class='ename '><span>3/1/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Surveying Technology'> <span>Surveying Technology</span> </td>
  <td nowrap width='168' class='ename'><span title='Bahirdar-Zegi (Kunzila Junction-Horticulture Farm-Zege Town Road Project)' >Bahirdar-Zegi (Kunzila Junction-</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>5,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>24</span></td>
<td nowrap width='150' class='ename'><span>Setegn Yenehun Beyene</span></td>
<td nowrap width='110'  class='ename '> <span>Junior Highway Enginee </span> </td>
<td nowrap width='45' class='ename '><span>3/1/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Metema-Abrajira Design & Build Road Project' >Metema-Abrajira Design & Build R</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>3,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>25</span></td>
<td nowrap width='150' class='ename'><span>Mekonen Lema Tirfe</span></td>
<td nowrap width='110'  class='ename '> <span>Junior Structure Engin </span> </td>
<td nowrap width='45' class='ename '><span>3/1/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Metema-Abrajira Design & Build Road Project' >Metema-Abrajira Design & Build R</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>3,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>26</span></td>
<td nowrap width='150' class='ename'><span>Ziyn Workie Truneh</span></td>
<td nowrap width='110'  class='ename '> <span>Junior Material Engine </span> </td>
<td nowrap width='45' class='ename '><span>3/1/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='-'> <span>-</span> </td>
  <td nowrap width='168' class='ename'><span title='Metema-Abrajira Design & Build Road Project' >Metema-Abrajira Design & Build R</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>3,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>27</span></td>
<td nowrap width='150' class='ename'><span>Tamirat Abiye Mamo</span></td>
<td nowrap width='110'  class='ename '> <span>Works Inspector </span> </td>
<td nowrap width='45' class='ename '><span>3/3/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Construction Technology & Management'> <span>Construction Technology &</span> </td>
  <td nowrap width='168' class='ename'><span title='Metema-Abrajira Design & Build Road Project' >Metema-Abrajira Design & Build R</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>3,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>28</span></td>
<td nowrap width='150' class='ename'><span>Medina Reshid Hussen</span></td>
<td nowrap width='110'  class='ename '> <span>Material Inspector </span> </td>
<td nowrap width='45' class='ename '><span>3/5/2018</span></td>
 <td nowrap width='22' class='ename '><span>Female</span></td>
  <td nowrap width='150' class='ename' title='Construction Technology'> <span>Construction Technology</span> </td>
  <td nowrap width='168' class='ename'><span title='Adaba-Angetu Road Project' >Adaba-Angetu Road Project</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>5,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>29</span></td>
<td nowrap width='150' class='ename'><span>Misrach Gebeyehu Kebede</span></td>
<td nowrap width='110'  class='ename '> <span>Secretery </span> </td>
<td nowrap width='45' class='ename '><span>3/6/2018</span></td>
 <td nowrap width='22' class='ename '><span>Female</span></td>
  <td nowrap width='150' class='ename' title='Secretary Science'> <span>Secretary Science</span> </td>
  <td nowrap width='168' class='ename'><span title='Teppi-Mizan Road Project' >Teppi-Mizan Road Project</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>2,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>30</span></td>
<td nowrap width='150' class='ename'><span>Demelash Asrat Amenu</span></td>
<td nowrap width='110'  class='ename '> <span>Works Inspector </span> </td>
<td nowrap width='45' class='ename '><span>3/6/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Building Concrete Work'> <span>Building Concrete Work</span> </td>
  <td nowrap width='168' class='ename'><span title='Hawasa-Hageremariam Section, Lot II Chuko-Yirgachefe' >Hawasa-Hageremariam Section, Lot</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>4,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>31</span></td>
<td nowrap width='150' class='ename'><span>Tadesse Worku Abebe</span></td>
<td nowrap width='110'  class='ename '> <span>Senior Surveyor </span> </td>
<td nowrap width='45' class='ename '><span>3/12/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Surveying Technology'> <span>Surveying Technology</span> </td>
  <td nowrap width='168' class='ename'><span title='Bonga-Ameya - Chida Road Upgrading Project Contract 2: Felege Selam-Ameya-Chida' >Bonga-Ameya - Chida Road Upgradi</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>5,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>32</span></td>
<td nowrap width='150' class='ename'><span>Nahom Getu Ketema</span></td>
<td nowrap width='110'  class='ename '> <span>Drafts person </span> </td>
<td nowrap width='45' class='ename '><span>3/15/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Adaba-Angetu Road Project' >Adaba-Angetu Road Project</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>4,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr></table><br><br> <table width='100%' class='sub_data table3' border='1'><tbody><tr>    <td width='306'>       <p>         <b><span>&nbsp;Prepared by ____________________</span></b></p> </td> <td width='286'>    <p>       <b><span>Approved By: _____________________</span></b></p> </td> <td width='62'>    <p>       &nbsp;</p> </td> </tr> <tr>    <td width='306'>       <p >          <b><span>&nbsp;Date _________________</span></b></p> </td> <td colspan='2' width='348'>    <p >       <b><span>Date ____________________</span></b></p> </td> </tr></tbody> </table> <br></div></div><div class='page'><div class='subpage' ><br><table width='100%' class='data' border='1'><tbody><tr><td>From:Jan 01, 2018-Jul 16, 2018</td><td> &nbsp;</td></tr><tr><td><p>Report type:Hired</p>
</td><td>
 <p class='MsoHeader'>
   <span>Page No.:</span></p>

 <p align='center' class='MsoHeader' style='text-align:center'>
 <span>Page </span><b><span>3</span></b><span> 
of </span><span><span><b>3</b></span></p></td></tr></tbody></table>  <table width="100%" class='data table4' border="1"> <tbody><tr align="center">
            <td rowspan="2" width="35">
                
                    S/No
            </td>
            <td rowspan="2" width="150">
                <p align="center" >
                    <b>Name</b></p>
            </td>
            <td rowspan="2" width="110">
                <p align="center" >
                    <b>Position</b></p>
            </td>
<td rowspan="2" width="45">
                <p align="center" >
                    <b>Hire Date</b></p>
            </td>
            <td rowspan="2" width="22">
                <p align="center" >
                    <b>Sex</b></p>
            </td>
            <td rowspan="2" width="150">
                <p align="center" >
                    <b>Education Background</b></p>
            </td>
            <td rowspan="2" width="170">
                <p align="center" >
                    <b>Work place</b></p>
            </td>
            <td rowspan="2" width="71">
                <p align="center" >
                    <b>Emply. Status</b></p>
            </td>
            <td colspan="2"width="71">
                <p align="center" >
                    <b>Gross Pay</b></p>
            </td>
            <td rowspan="2" width="71">
                <p align="center" >
                    <b>Remark</b></p>
            </td>
        </tr>
         <tr style="line-height: 12px;" align="center">
            <td width="75">
                <p align="center" >
                    <b>Salary</b></p>
            </td>
            <td width="71">
                <p align="center" >
                    <b>Alw/Pr</b></p>
            </td>
        </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>33</span></td>
<td nowrap width='150' class='ename'><span>Sisay Desalegn Gebregziabher</span></td>
<td nowrap width='110'  class='ename '> <span>Senior Surveyor </span> </td>
<td nowrap width='45' class='ename '><span>3/15/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Surveying Technology'> <span>Surveying Technology</span> </td>
  <td nowrap width='168' class='ename'><span title='Bahirdar-Zegi (Kunzila Junction-Horticulture Farm-Zege Town Road Project)' >Bahirdar-Zegi (Kunzila Junction-</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>6,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>34</span></td>
<td nowrap width='150' class='ename'><span>Marye Misganaw Beza</span></td>
<td nowrap width='110'  class='ename '> <span>Right of Way Managment </span> </td>
<td nowrap width='45' class='ename '><span>3/19/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Land Adminstration'> <span>Land Adminstration</span> </td>
  <td nowrap width='168' class='ename'><span title='Teppi-Mizan Road Project' >Teppi-Mizan Road Project</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>7,054.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>35</span></td>
<td nowrap width='150' class='ename'><span>Gosa Bekele Gudeta</span></td>
<td nowrap width='110'  class='ename '> <span>Surveyor </span> </td>
<td nowrap width='45' class='ename '><span>3/19/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='10th Grade'> <span>10th Grade</span> </td>
  <td nowrap width='168' class='ename'><span title='Adama-Awash Expressway Project' >Adama-Awash Expressway Project</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>4,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>36</span></td>
<td nowrap width='150' class='ename'><span>Mesfin Yeshanew Desalgn</span></td>
<td nowrap width='110'  class='ename '> <span>Quantity Surveyor </span> </td>
<td nowrap width='45' class='ename '><span>3/20/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Advanced Building Construction Technician'> <span>Advanced Building Constru</span> </td>
  <td nowrap width='168' class='ename'><span title='Fiyel-Weha-Abi Adi Road Project, Contract 1: Fiyel Weha-Tekeze River ' >Fiyel-Weha-Abi Adi Road Project,</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>6,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>1,500.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>37</span></td>
<td nowrap width='150' class='ename'><span>Eliyas Mohammd Ali</span></td>
<td nowrap width='110'  class='ename '> <span>Quantity Surveyor </span> </td>
<td nowrap width='45' class='ename '><span>3/20/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Drafting Technology'> <span>Drafting Technology</span> </td>
  <td nowrap width='168' class='ename'><span title='Bedele-Metu Road Upgrading Project' >Bedele-Metu Road Upgrading Proje</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>6,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>38</span></td>
<td nowrap width='150' class='ename'><span>Shambel Agizew Demissie</span></td>
<td nowrap width='110'  class='ename '> <span>Works Inspector </span> </td>
<td nowrap width='45' class='ename '><span>3/26/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil & Urban Engineering'> <span>Civil & Urban Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Metema-Abrajira Design & Build Road Project' >Metema-Abrajira Design & Build R</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>4,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>39</span></td>
<td nowrap width='150' class='ename'><span>Samuel Tesfaye Atinafe</span></td>
<td nowrap width='110'  class='ename '> <span>Assistant Resident Eng </span> </td>
<td nowrap width='45' class='ename '><span>4/2/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Sodo-Sawla roads upgrading project, lot 2: Sodo-Dinke Road Section' >Sodo-Sawla roads upgrading proje</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>20,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>40</span></td>
<td nowrap width='150' class='ename'><span>Naod Terefe Yetimgeta</span></td>
<td nowrap width='110'  class='ename '> <span>Resident Engineer </span> </td>
<td nowrap width='45' class='ename '><span>4/2/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Metema-Abrajira Design & Build Road Project' >Metema-Abrajira Design & Build R</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>19,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>41</span></td>
<td nowrap width='150' class='ename'><span>Nebiyu Terefe Reta</span></td>
<td nowrap width='110'  class='ename '> <span>Material Inspector </span> </td>
<td nowrap width='45' class='ename '><span>4/12/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Masonry'> <span>Masonry</span> </td>
  <td nowrap width='168' class='ename'><span title='Bonga-Ameya - Chida Road Upgrading Project Contract 2: Felege Selam-Ameya-Chida' >Bonga-Ameya - Chida Road Upgradi</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>5,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>42</span></td>
<td nowrap width='150' class='ename'><span>Mikiyas Simaneh Shitahune</span></td>
<td nowrap width='110'  class='ename '> <span>Junior Structure Engin </span> </td>
<td nowrap width='45' class='ename '><span>4/14/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Menebegna-Fincha-Shambu-Bako  Road Upgrading Project , Contract 1 : Menebegna-Fincha-Shambu' >Menebegna-Fincha-Shambu-Bako  Ro</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>3,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>43</span></td>
<td nowrap width='150' class='ename'><span>Yitayal Tesema Fetene</span></td>
<td nowrap width='110'  class='ename '> <span>Junior Quanitity Surve </span> </td>
<td nowrap width='45' class='ename '><span>4/14/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Menebegna-Fincha-Shambu-Bako  Road Upgrading Project , Contract 1 : Menebegna-Fincha-Shambu' >Menebegna-Fincha-Shambu-Bako  Ro</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>3,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>44</span></td>
<td nowrap width='150' class='ename'><span>Demetros Peteros Assefa</span></td>
<td nowrap width='110'  class='ename '> <span>Junior Highway Enginee </span> </td>
<td nowrap width='45' class='ename '><span>4/18/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Fiyel-Weha-Abi Adi Road Project, Contract 1: Fiyel Weha-Tekeze River ' >Fiyel-Weha-Abi Adi Road Project,</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>3,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>45</span></td>
<td nowrap width='150' class='ename'><span>Mohammed Ahmed Rega</span></td>
<td nowrap width='110'  class='ename '> <span>Junior Claim Expert </span> </td>
<td nowrap width='45' class='ename '><span>4/18/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Fiyel-Weha-Abi Adi Road Project, Contract 1: Fiyel Weha-Tekeze River ' >Fiyel-Weha-Abi Adi Road Project,</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>3,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>46</span></td>
<td nowrap width='150' class='ename'><span>Samson Gebremeskel berha</span></td>
<td nowrap width='110'  class='ename '> <span>Junior Material Engine </span> </td>
<td nowrap width='45' class='ename '><span>4/24/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Fiyel-Weha-Abi Adi Road Project, Contract 1: Fiyel Weha-Tekeze River ' >Fiyel-Weha-Abi Adi Road Project,</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>3,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>47</span></td>
<td nowrap width='150' class='ename'><span>Essa Awol Hassen</span></td>
<td nowrap width='110'  class='ename '> <span>Junior Quanitity Surve </span> </td>
<td nowrap width='45' class='ename '><span>4/25/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Metema-Abrajira Design & Build Road Project' >Metema-Abrajira Design & Build R</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>3,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>48</span></td>
<td nowrap width='150' class='ename'><span>Leilti Gebreegziabher Mezgebo</span></td>
<td nowrap width='110'  class='ename '> <span>Junior Claim Expert </span> </td>
<td nowrap width='45' class='ename '><span>4/25/2018</span></td>
 <td nowrap width='22' class='ename '><span>Female</span></td>
  <td nowrap width='150' class='ename' title='Civil Engineering'> <span>Civil Engineering</span> </td>
  <td nowrap width='168' class='ename'><span title='Metema-Abrajira Design & Build Road Project' >Metema-Abrajira Design & Build R</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>3,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>49</span></td>
<td nowrap width='150' class='ename'><span>Matiwos Enyew Bogale</span></td>
<td nowrap width='110'  class='ename '> <span>Right of Way Managment </span> </td>
<td nowrap width='45' class='ename '><span>5/1/2018</span></td>
 <td nowrap width='22' class='ename '><span>Male</span></td>
  <td nowrap width='150' class='ename' title='Land Adminstration'> <span>Land Adminstration</span> </td>
  <td nowrap width='168' class='ename'><span title='Metema-Abrajira Design & Build Road Project' >Metema-Abrajira Design & Build R</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>7,054.00</span></td>
<td nowrap width='71' class='cssamt'><span>2,000.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>50</span></td>
<td nowrap width='150' class='ename'><span>Merhawit Zeru Kahsay</span></td>
<td nowrap width='110'  class='ename '> <span>Drafts person </span> </td>
<td nowrap width='45' class='ename '><span>5/8/2018</span></td>
 <td nowrap width='22' class='ename '><span>Female</span></td>
  <td nowrap width='150' class='ename' title='Drafting Technology'> <span>Drafting Technology</span> </td>
  <td nowrap width='168' class='ename'><span title='Hawasa-Hageremariam Section, Lot II Chuko-Yirgachefe' >Hawasa-Hageremariam Section, Lot</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>5,000.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr> <tr  class='repeatable-data-row'>
 <td nowrap width='35' class='ename'><span>51</span></td>
<td nowrap width='150' class='ename'><span>Frezer Getachew Zena</span></td>
<td nowrap width='110'  class='ename '> <span>Secretery </span> </td>
<td nowrap width='45' class='ename '><span>5/11/2018</span></td>
 <td nowrap width='22' class='ename '><span>Female</span></td>
  <td nowrap width='150' class='ename' title='Secreterial Science & Office Management'> <span>Secreterial Science & Off</span> </td>
  <td nowrap width='168' class='ename'><span title='Metema-Abrajira Design & Build Road Project' >Metema-Abrajira Design & Build R</span>      </td>
 <td nowrap width='22' class='ename '><span>active</span></td>
<td nowrap width='71' class='cssamt'><span>2,500.00</span></td>
<td nowrap width='71' class='cssamt'><span>0.00</span></td>
<td nowrap width='71' align='center' ><span>&nbsp;</span></td>
 </tr></table><br><br> <table width='100%' class='sub_data table3' border='1'><tbody><tr>    <td width='306'>       <p>         <b><span>&nbsp;Prepared by ____________________</span></b></p> </td> <td width='286'>    <p>       <b><span>Approved By: _____________________</span></b></p> </td> <td width='62'>    <p>       &nbsp;</p> </td> </tr> <tr>    <td width='306'>       <p >          <b><span>&nbsp;Date _________________</span></b></p> </td> <td colspan='2' width='348'>    <p >       <b><span>Date ____________________</span></b></p> </td> </tr></tbody> </table> <br></div></div><div class='page'><div class='subpage' ><table cellspacing=0 cellpadding=0><tr><td colspan='3'>Summery: Gender</td></tr><tr><td>Male</td><td>:</td><td>44</td></tr><tr><td>Female</td><td>:</td><td>7</td></tr></table><br><table cellspacing=0 cellpadding=0><tr><td colspan=4>Summery by Position</td></tr><tr><td>no.</td><td>Position</td><td></td><td>No. Emp.</td></tr>
<tr><td> 1</td><td>Assistant Resident Engineer</td><td>:</td><td> 3</td></tr>
<tr><td> 2</td><td>Contract Engineer</td><td>:</td><td> 1</td></tr>
<tr><td> 3</td><td>Drafts person</td><td>:</td><td> 2</td></tr>
<tr><td> 4</td><td>Hydrologist</td><td>:</td><td> 1</td></tr>
<tr><td> 5</td><td>Junior Claim Expert</td><td>:</td><td> 2</td></tr>
<tr><td> 6</td><td>Junior Highway Engineer</td><td>:</td><td> 2</td></tr>
<tr><td> 7</td><td>Junior Material Engineer</td><td>:</td><td> 2</td></tr>
<tr><td> 8</td><td>Junior Quanitity Surveyor</td><td>:</td><td> 2</td></tr>
<tr><td> 9</td><td>Junior Structure Engineer</td><td>:</td><td> 2</td></tr>
<tr><td> 10</td><td>Labratory Technician</td><td>:</td><td> 1</td></tr>
<tr><td> 11</td><td>Material Engineer</td><td>:</td><td> 1</td></tr>
<tr><td> 12</td><td>Material Inspector</td><td>:</td><td> 3</td></tr>
<tr><td> 13</td><td>Office attendant</td><td>:</td><td> 1</td></tr>
<tr><td> 14</td><td>Quantity Surveyor</td><td>:</td><td> 2</td></tr>
<tr><td> 15</td><td>Right of Way Managment Specialist</td><td>:</td><td> 3</td></tr>
<tr><td> 16</td><td>Secretery</td><td>:</td><td> 3</td></tr>
<tr><td> 17</td><td>Senior Labratory Technician</td><td>:</td><td> 1</td></tr>
<tr><td> 18</td><td>Senior Surveyor</td><td>:</td><td> 4</td></tr>
<tr><td> 19</td><td>Structural Inspector</td><td>:</td><td> 1</td></tr>
<tr><td> 20</td><td>Works Inspector</td><td>:</td><td> 5</td></tr>
</table>
</div></div>
</div><div style='clear:left;'></div><br>00|00||
            </div>

    <table id='hrdftrtbl' border='0' cellspacing='0' cellpadding='0'>
    <tr><td>        <div style='mso-element:header' id=h1 >
        <!-- HEADER-tags -->
             <table width='100%' class='sub_data table3' border='1'><tbody>
<tr>
<td width='20%' >
 <p class='MsoHeader'>
<img align='left' height='41' hspace='12' 
 src = 'http://kirsoft:8015/images/netlog.png'v:shapes='Picture_x0020_9' width='69' /><span></span></p>
  </td><td valign='top' width='50%'>
  <p class='MsoHeader'>
   <b><span>&nbsp;</span></b><span>Company Name:</span></p>
 <p align='center'  style='text-align:center' class='MsoHeader' ><span><%=Session("company_name_amharic") %></span></p>
 <p align='center'  class='MsoHeader' style='text-align:center'><b><span><%=Session("company_name") %></span></b><span></span></p>
</td><td colspan='2' valign='top' width='30%'>
    <p class='MsoHeader'>
 <span>Document No.:</span></p>
 <p class='MsoHeader'>
<span></span></p>
 <p align='center' class='MsoHeader' style='text-align:center'>
   <span>OF/NET/______</span></p>
 </td>        </tr>
<tr>         <td colspan='2' valign='top' width='70%'>
      <p class='MsoHeader'>
<span>Title:&nbsp; </span>
</p>
 <p align='center' class='MsoHeader' style='text-align:center'>
 <b><span>Human Resource Report</span></b><span></span></p>
  </td>
<td valign='top' width='10%'>
  <p class='MsoHeader'>
  <span>Issue No.:</span></p>
  <p class='MsoHeader'>
   <span></span></p>
 <p align='center' class='MsoHeader' style='text-align:center'>
  <span>1</span></p>
  <p class='MsoHeader'>
  <span></span></p>
  </td>
 <td valign='top' width='20%'>
  </td>
</tr>
<tbody></table><br><table width='100%' class='data' border='1'><tbody><tr><td>From:Jan 01, 2018-Jul 16, 2018</td><td> &nbsp;</td></tr><tr><td><p>Report type:Hired</p>
</td><td>
 <p class='MsoHeader'>
   <span>Page No.:</span></p>

 <p align='center' class='MsoHeader' style='text-align:center'>
 <span>Page </span><b><span>1</span></b><span> 
of </span><span><span><b>3</b></span></p></td></tr></tbody></table><p class=MsoHeader >HEADER </p>
        <!-- end HEADER-tags -->
        </div>
    </td>
    <td>
    <div style='mso-element:footer' id=f1>
    <span style='position:relative;z-index:-1'> 
        <!-- FOOTER-tags -->
        FOOTER

        <span style='mso-no-proof:yes'><!--[if gte vml 1]><v:shapetype
         id="_x0000_t75" coordsize="21600,21600" o:spt="75" o:preferrelative="t"
         path="m@4@5l@4@11@9@11@9@5xe" filled="f" stroked="f">
         <v:formulas>
          <v:f eqn="if lineDrawn pixelLineWidth 0"/>
          <v:f eqn="sum @0 1 0"/>
          <v:f eqn="sum 0 0 @1"/>
          <v:f eqn="prod @2 1 2"/>
          <v:f eqn="prod @3 21600 pixelWidth"/>
          <v:f eqn="prod @3 21600 pixelHeight"/>
          <v:f eqn="sum @0 0 1"/>
          <v:f eqn="prod @6 1 2"/>
          <v:f eqn="prod @7 21600 pixelWidth"/>
          <v:f eqn="sum @8 21600 0"/>
          <v:f eqn="prod @7 21600 pixelHeight"/>
          <v:f eqn="sum @10 21600 0"/>
         </v:formulas>
         <v:path o:extrusionok="f" gradientshapeok="t" o:connecttype="rect"/>
         <o:lock v:ext="edit" aspectratio="t"/>
        </v:shapetype><v:shape id="Picture_x0020_1" o:spid="_x0000_s3073" type="#_x0000_t75"
         alt="VHB" style='position:absolute;
         margin-right:0pt;margin-top:-400pt;
         z-index:-1;
         visibility:visible;mso-wrap-style:square;mso-wrap-distance-left:9pt;
         mso-wrap-distance-top:0;mso-wrap-distance-right:9pt;
         mso-wrap-distance-bottom:0;mso-position-horizontal:absolute;
         mso-position-horizontal-relative:text;mso-position-vertical:absolute;
         mso-position-vertical-relative:text'>
         <v:imagedata src=""/>
        </v:shape><![endif]--></span>
           <p class=MsoFooter>
           <span style=mso-tab-count:2'></span>
           Page <span style='mso-field-code: PAGE '><span style='mso-no-proof:yes'></span> from <span style='mso-field-code: NUMPAGES '></span>

        <!-- end FOOTER-tags -->
   </span>


        </p>
    </div>



    <div style='mso-element:header' id=fh1>
        <p class=MsoHeader><span lang=EN-US style='mso-ansi-language:EN-US'>&nbsp;<o:p></o:p></span></p>
        </div>
        <div style='mso-element:footer' id=ff1>
        <p class=MsoFooter><span lang=EN-US style='mso-ansi-language:EN-US'>&nbsp;<o:p></o:p></span></p>
    </div>

    </td></tr>
    </table>
</div>

</body></html>