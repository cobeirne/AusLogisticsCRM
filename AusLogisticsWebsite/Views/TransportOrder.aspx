<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransportOrder.aspx.cs" Inherits="AusLogisticsWebsite.Default" Theme="AusLogisticsTheme" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AL | Transport Order</title>
    <link rel="stylesheet" type="text/css" href="AusLogisticsStyles.css" />
</head>
<body>
    <header>
        <img class="logo" src="../Images/AusLogisticsLogoBlueOnWhite.png" />
        <div class="company-name">Australian Logistics CRM</div>
    </header>

    <section>
        <h1>Transport Order</h1>
        <form id="trasnportOrder" runat="server">
            <article>
                <h2>Customer Details</h2>
                <table>
                    <tr>
                        <td class="form-label">Membership:</td>
                        <td>
                            <asp:RadioButtonList ID="rblMembership" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblMembership_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="0">New</asp:ListItem>
                                <asp:ListItem Value="1">Existing</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="form-label">Member ID:</td>
                        <td>
                            <asp:TextBox id="tbMemberId" size="5" maxlength="5" runat="server" enabled="false" />
                            <asp:Button ID="btnSearchMember" runat="server" Text="Search" OnClick="btnSearchMember_Click" enabled="false" validationGroup="existingSearch" />
                            <div class="val-error">
                                <asp:RegularExpressionValidator id="valMemberId" runat="server" 
                                    ControlToValidate="tbMemberId" 
                                    ErrorMessage="Invalid Membership Number" 
                                    ValidationExpression="^[0-9]{1,5}$"
                                    forecolor="red"
                                    validationGroup="existingSearch">
                                </asp:RegularExpressionValidator>
                                <asp:Label ID="lblMembershipError" runat="server" visible="false" ForeColor ="red" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="form-label">Member Class:</td>
                        <td>
                            <asp:TextBox id="tbMemberClass" runat="server" readonly="true" enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="form-label">First Name:</td>
                        <td>
                            <asp:TextBox id="tbFirstName" runat="server" size="34" maxlength="34"/>
                            <div class="val-error">
                                <asp:RegularExpressionValidator id="valFirstName" runat="server" 
                                    ControlToValidate="tbFirstName" 
                                    ErrorMessage="Invalid First Name" 
                                    ValidationExpression="^[a-zA-Z-]*$"
                                    forecolor="red"
                                    Display="Dynamic">
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="valFirstNameReq" runat="server"
                                    ControlToValidate="tbFirstName"
                                    ErrorMessage="Required Field"
                                    forecolor="red"
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td><div class="input-hint">e.g. John or Mary-Jane</div></td>
                    </tr>
                    <tr>
                        <td class="form-label">Last Name:</td>
                        <td>
                            <asp:TextBox id="tbLastName" size="34" maxlength="34" runat="server" />
                            <div class="val-error">
                                <asp:RegularExpressionValidator id="valLastName" runat="server" 
                                    ControlToValidate="tbLastName" 
                                    ErrorMessage="Invalid Last Name" 
                                    ValidationExpression="^[a-zA-Z-']*$"
                                    Display="Dynamic"
                                    forecolor="red">
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="valLastNameReq" runat="server"
                                    ControlToValidate="tbLastName"
                                    ErrorMessage="Required Field"
                                    forecolor="red"
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td><div class="input-hint">e.g. White or Smith-Jones</div></td>
                    </tr>
                </table>
            </article>  


            <article>
                <h2>Pickup Details</h2>
                <table>
                    <tr>
                        <td class="form-label">Unit:</td>
                        <td>
                            <asp:TextBox id="tbPickupUnit" size="4" maxlength="4" runat="server" />
                            <div class="val-error">
                                <asp:RegularExpressionValidator id="revPickupUnit" runat="server" 
                                    ControlToValidate="tbPickupUnit" 
                                    ErrorMessage="Invalid Unit Number" 
                                    ValidationExpression="^[0-9a-z-A-Z]{1,4}$"
                                    forecolor="red">
                                </asp:RegularExpressionValidator>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td><div class="input-hint">e.g. 1, A (Optional)</div></td>
                    </tr>
                    <tr>
                        <td class="form-label">Number:</td>
                        <td>
                            <asp:TextBox id="tbPickupNumber" size="4" maxlength="4" runat="server" />
                            <div class="val-error">
                                <asp:RegularExpressionValidator id="revPickupNumber" runat="server" 
                                    ControlToValidate="tbPickupNumber" 
                                    ErrorMessage="Invalid Address Number" 
                                    ValidationExpression="^[0-9]{1,4}$"
                                    Display="Dynamic"
                                    forecolor="red">
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="revPickupNumberReq" runat="server"
                                    ControlToValidate="tbPickupNumber"
                                    ErrorMessage="Required Field"
                                    forecolor="red"
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>                                                                       
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td><div class="input-hint">e.g. 1234</div></td>
                    </tr>
                    <tr>
                        <td class="form-label">Address:</td>
                        <td>
                            <asp:TextBox id="tbPickupAddress" size="34" maxlength="34" runat="server" />
                            <div class="val-error">
                                <asp:RegularExpressionValidator id="revPickupAddress" runat="server" 
                                    ControlToValidate="tbPickupAddress" 
                                    ErrorMessage="Invalid Address" 
                                    ValidationExpression="^[a-zA-Z]+ [a-zA-Z]+$"
                                    Display="Dynamic"
                                    forecolor="red">
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="revPickupAddressReq" runat="server"
                                    ControlToValidate="tbPickupAddress"
                                    ErrorMessage="Required Field"
                                    forecolor="red"
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>                                                                      
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td><div class="input-hint">e.g. Main Street</div></td>
                    </tr>
                    <tr>
                        <td class="form-label">State:</td>
                        <td>
                            <asp:DropDownList id="ddPickupState" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="form-label">Suburb:</td>
                        <td>
                            <asp:TextBox id="tbPickupSuburb" size="34" maxlength="34" runat="server" />
                            <div class="val-error">
                                <asp:CustomValidator id="cvPickupSuburb" runat="server" 
                                  OnServerValidate="ValidatePickupSuburb" 
                                  ControlToValidate="tbPickupSuburb" 
                                  ErrorMessage="Invalid Suburb"
                                  Display="Dynamic"
                                  Forecolor="Red">
                                </asp:CustomValidator>      
                                <asp:RequiredFieldValidator ID="cvPickupSuburbReq" runat="server"
                                    ControlToValidate="tbPickupSuburb"
                                    ErrorMessage="Required Field"
                                    forecolor="red"
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>                                                                     
                            </div>
                       </td>
                    </tr>
                    <tr>
                        <td />
                        <td><div class="input-hint">e.g. Melbourne</div></td>
                    </tr>
                    <tr>
                        <td class="form-label">Post Code:</td>
                        <td>
                            <asp:TextBox id="tbPickupPostCode" size="4" maxlength="4" runat="server" />
                            <div class="val-error">
                                <asp:CustomValidator id="cvPickupPostCode" runat="server" 
                                  OnServerValidate="ValidatePickupPostCode" 
                                  ControlToValidate="tbPickupPostCode" 
                                  ErrorMessage="Invalid Post Code"
                                  Display="Dynamic"
                                  Forecolor="Red">
                                </asp:CustomValidator>         
                                <asp:RequiredFieldValidator ID="cvPickupPostCodeReq" runat="server"
                                    ControlToValidate="tbPickupPostCode"
                                    ErrorMessage="Required Field"
                                    forecolor="red"
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>                                                                     

                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td><div class="input-hint">e.g. 3000</div></td>
                    </tr>
                </table>
            </article>  

            <article>
                <h2>Delivery Details</h2>
                <table>
                    <tr>
                        <td class="form-label">Unit:</td>
                        <td>
                            <asp:TextBox id="tbDeliveryUnit" size="4" maxlength="4" runat="server" />
                            <div class="val-error">
                                <asp:RegularExpressionValidator id="revDeliveryUnit" runat="server" 
                                    ControlToValidate="tbDeliveryUnit" 
                                    ErrorMessage="Invalid Unit Number" 
                                    ValidationExpression="^[0-9a-z-A-Z]{1,4}$"
                                    forecolor="red">
                                </asp:RegularExpressionValidator>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td><div class="input-hint">e.g. 1, A (Optional)</div></td>
                    </tr>
                    <tr>
                        <td class="form-label">Number:</td>
                        <td>
                            <asp:TextBox id="tbDeliveryNumber" size="4" maxlength="4" runat="server" />
                            <div class="val-error">
                                <asp:RegularExpressionValidator id="revDeliveryNumber" runat="server" 
                                    ControlToValidate="tbDeliveryNumber" 
                                    ErrorMessage="Invalid Address Number" 
                                    ValidationExpression="^[0-9]{1,4}$"
                                    Display="Dynamic" 
                                    forecolor="red">
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="revDeliveryNumberReq" runat="server"
                                    ControlToValidate="tbDeliveryNumber"
                                    ErrorMessage="Required Field"
                                    forecolor="red"
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>                                                                    
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td><div class="input-hint">e.g. 1234</div></td>
                    </tr>
                    <tr>
                        <td class="form-label">Address:</td>
                        <td>
                            <asp:TextBox id="tbDeliveryAddress" size="34" maxlength="34" runat="server" />
                            <div class="val-error">
                                <asp:RegularExpressionValidator id="revDeliveryAddress" runat="server" 
                                    ControlToValidate="tbDeliveryAddress" 
                                    ErrorMessage="Invalid Address" 
                                    ValidationExpression="^[a-zA-Z]+ [a-zA-Z]+$"
                                    Display="Dynamic"
                                    forecolor="red">
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="tbDeliveryAddressReq" runat="server"
                                    ControlToValidate="tbDeliveryAddress"
                                    ErrorMessage="Required Field"
                                    forecolor="red"
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>                                                                    
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td><div class="input-hint">e.g. Main Street</div></td>
                    </tr>
                    <tr>
                        <td class="form-label">State:</td>
                        <td>
                            <asp:DropDownList id="ddDeliveryState" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="form-label">Suburb:</td>
                        <td>
                            <asp:TextBox id="tbDeliverySuburb" size="34" maxlength="34" runat="server" />
                            <div class="val-error">
                                <asp:CustomValidator id="cvDeliverySuburb" runat="server" 
                                  OnServerValidate="ValidateDeliverySuburb" 
                                  ControlToValidate="tbDeliverySuburb" 
                                  ErrorMessage="Invalid Suburb"
                                  Display="Dynamic"
                                  Forecolor="Red">
                                </asp:CustomValidator>         
                                <asp:RequiredFieldValidator ID="cvDeliverySuburbReq" runat="server"
                                    ControlToValidate="tbDeliverySuburb"
                                    ErrorMessage="Required Field"
                                    forecolor="red"
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>                                                                    
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td><div class="input-hint">e.g. Melbourne</div></td>
                    </tr>
                    <tr>
                        <td class="form-label">Post Code:</td>
                        <td>
                            <asp:TextBox id="tbDeliveryPostCode" size="4" maxlength="4" runat="server" />
                            <div class="val-error">
                                <asp:CustomValidator id="cvDeliveryPostCode" runat="server" 
                                  OnServerValidate="ValidateDeliveryPostCode" 
                                  ControlToValidate="tbDeliveryPostCode" 
                                  ErrorMessage="Invalid Post Code"
                                  Display="Dynamic"
                                  Forecolor="Red">
                                </asp:CustomValidator>   
                                <asp:RequiredFieldValidator ID="cvDeliveryPostCodeReq" runat="server"
                                    ControlToValidate="tbDeliveryPostCode"
                                    ErrorMessage="Required Field"
                                    forecolor="red"
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>                                                                    
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td><div class="input-hint">e.g. 3000</div></td>
                    </tr>
                </table>
            </article>  

            <article>
                <h2>Consignment Details</h2>
                <table>
                    <tr>
                        <td class="form-label">Delivery Weight:</td>
                        <td>
                            <asp:TextBox id="tbDeliveryWeight" size="5" maxlength="5" runat="server" /><div class="input-unit">kg</div>
                            <div class="val-error">
                                <asp:RangeValidator id="rvDeliveryWeight" runat="server"
                                    ControlToValidate="tbDeliveryWeight"
                                    MinimumValue="1000"
                                    MaximumValue="50000"
                                    Type="Integer"
                                    Text="Invalid weight"
                                    Display="Dynamic"
                                    forecolor="red">
                                </asp:RangeValidator>  
                                <asp:RequiredFieldValidator ID="rvDeliveryWeightReq" runat="server"
                                    ControlToValidate="tbDeliveryWeight"
                                    ErrorMessage="Required Field"
                                    forecolor="red"
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>                                                                    
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td><div class="input-hint">Min 1000kg to Max 50000kg (Round up to nearest whole number)</div></td>
                    </tr>
                    <tr>
                        <td class="form-label">Delivery Date:</td>
                        <td>
                            <asp:Calendar ID="calDelivery" SelectionMode="Day" ShowGridLines="True" runat="server"></asp:Calendar>
                        </td>
                    </tr>
                    <tr>
                        <td class="form-label">Delivery Time:</td>
                        <td>
                            <asp:DropDownList ID="ddDeliveryHour" runat="server" /><div class="input-unit">Hour</div>
                            <asp:DropDownList ID="ddDeliveryMinute" runat="server" /><div class="input-unit">Minutes</div>

                            <div class="val-error">
                                <asp:CustomValidator id="cvDeliveryDate" runat="server" 
                                  OnServerValidate="ValidateDeliveryTime" 
                                  ControlToValidate="ddDeliveryMinute" 
                                  ErrorMessage="Unsufficient time to deliver"
                                  Forecolor="Red">
                                </asp:CustomValidator>         
                            </div>
                        </td>                  

                    </tr>
                </table>
            </article> 
            <asp:Button id="btFormSubmit" Text="Submit Order" runat="server" OnClick="btFormSubmit_Click" />
        </form>
    </section>

    <footer>
        <p>&copy; 2016 Australian Logisitics</p>
    </footer>
</body>
</html>
