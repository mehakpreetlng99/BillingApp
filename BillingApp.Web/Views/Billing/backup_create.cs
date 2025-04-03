//@using BillingApp.DTO
//@model BillingApp.DTO.InvoiceDTO

//@{
//    ViewData["Title"] = "Create Invoice";
//}


//<h2>Create Invoice</h2>

//<form id="billingForm" asp-action="GenerateInvoice" asp-controller="Billing" method="post">
//    <div class="form-group">
//        <label for="CustomerPhone">Customer Phone</label>
//        <input type="text" id="customerPhone" class="form-control" asp-for="CustomerPhone" placeholder="Enter phone number" required>
//    </div>
//    <div class="form-group">
//        <label for="CustomerName">Customer Name</label>
//        <input type="text" id="customerName" class="form-control" asp-for="CustomerName" placeholder="Enter the name " required>
//    </div>
    
//    <div class="form-group">
//        <label for="product">Product</label>
//        @* <select id="product" class="form-control" required> *@
//        @*     <option value="">Select Product</option> *@
//        @*     @foreach (var product in Model.Products) *@
//        @*     { *@
//        @*         <option value="@product.Id" data-price="@product.Price">@product.Name</option> *@
//        @*     } *@
//        @* </select> *@
//        <select id="product" class="form-control" required>
//            <option value="">Select Product</option>
//            @if (ViewBag.Products != null)
//            {
//                @foreach (var product in ViewBag.Products as List<ProductDTO>)
//                {
//                    <option value="@product.Id" data-price="@product.Price">@product.Name</option>
//                }
//            }
//            @* @foreach (var product in ViewBag.Products as List<ProductDTO>) *@
//            @* { *@
//            @*     <option value="@product.Id" data-price="@product.Price">@product.Name</option> *@
//            @* } *@
//        </select>

//    </div>
    
//    <div class="form-group">
//        <label for="price">Price</label>
//        <input type="text" id="price" class="form-control" readonly>
//    </div>
    
//    <div class="form-group">
//        <label for="quantity">Quantity</label>
//        <input type="number" id="quantity" class="form-control" min="1" value="1">
//    </div>
    
//    <div class="form-group">
//        <label for="subtotal">Subtotal</label>
//        <input type="text" id="subtotal" class="form-control" readonly>
//    </div>
    
//    <div class="form-group">
//        <label for="gst">GST (%)</label>
//        <input type="number" id="gst" class="form-control" min="0" value="5">
//    </div>
    
//    <div class="form-group">
//        <label for="discount">Discount (%)</label>
//        <input type="number" id="discount" class="form-control" min="0" value="0">
//    </div>
    
//    <div class="form-group">
//        <label for="total">Total</label>
//        <input type="text" id="total" class="form-control" readonly>
//    </div>
    
//    @* <form asp-action="GenerateInvoice" asp-controller="Billing" method="post"> *@
//    @*     <input type="hidden" asp-for="Id" /> *@
//    @*     <input type="hidden" asp-for="CustomerId" /> *@
//    @*     <input type="hidden" asp-for="TotalAmount" /> *@

//        <button type="submit" class="btn btn-success">Generate Invoice</button>
//    @* </form> *@

//</form>

//<script>
//    document.addEventListener("DOMContentLoaded", function () {
//        const productDropdown = document.getElementById("product");
//        const priceInput = document.getElementById("price");
//        const quantityInput = document.getElementById("quantity");
//        const subtotalInput = document.getElementById("subtotal");
//        const gstInput = document.getElementById("gst");
//        const discountInput = document.getElementById("discount");
//        const totalInput = document.getElementById("total");
        
//        function calculateTotal() {
//            let price = parseFloat(priceInput.value) || 0;
//            let quantity = parseInt(quantityInput.value) || 1;
//            let gst = parseFloat(gstInput.value) || 0;
//            let discount = parseFloat(discountInput.value) || 0;
            
//            let subtotal = price * quantity;
//            let gstAmount = (subtotal * gst) / 100;
//            let discountAmount = (subtotal * discount) / 100;
//            let total = subtotal + gstAmount - discountAmount;
            
//            subtotalInput.value = subtotal.toFixed(2);
//            totalInput.value = total.toFixed(2);
//        }
        
//        productDropdown.addEventListener("change", function () {
//            let selectedOption = productDropdown.options[productDropdown.selectedIndex];
//            let price = selectedOption.getAttribute("data-price");
//            priceInput.value = price;
//            calculateTotal();
//        });
        
//        quantityInput.addEventListener("input", calculateTotal);
//        gstInput.addEventListener("input", calculateTotal);
//        discountInput.addEventListener("input", calculateTotal);
//    });

//    document.getElementById("billingForm").addEventListener("submit", function(event) {
//        console.log("Form submitted!"); // Debugging
//    });
//</script>
//@using BillingApp.DTO
//@model BillingApp.DTO.InvoiceDTO

//@{
//    ViewData["Title"] = "Create Invoice";
//}


//<h2>Create Invoice</h2>

//<form id="billingForm" asp-action="GenerateInvoice" asp-controller="Billing" method="post">
//    <div class="form-group">
//        <label for="CustomerPhone">Customer Phone</label>
//        <input type="text" id="customerPhone" class="form-control" asp-for="CustomerPhone" placeholder="Enter phone number" required>
//    </div>
//    <div class="form-group">
//        <label for="CustomerName">Customer Name</label>
//        <input type="text" id="customerName" class="form-control" asp-for="CustomerName" placeholder="Enter the name " required>
//    </div>
    
//    <div class="form-group">
//        <label for="product">Product</label>
//        @* <select id="product" class="form-control" required> *@
//        @*     <option value="">Select Product</option> *@
//        @*     @foreach (var product in Model.Products) *@
//        @*     { *@
//        @*         <option value="@product.Id" data-price="@product.Price">@product.Name</option> *@
//        @*     } *@
//        @* </select> *@
//        <select id="product" class="form-control" required>
//            <option value="">Select Product</option>
//            @if (ViewBag.Products != null)
//            {
//                @foreach (var product in ViewBag.Products as List<ProductDTO>)
//                {
//                    <option value="@product.Id" data-price="@product.Price">@product.Name</option>
//                }
//            }
//            @* @foreach (var product in ViewBag.Products as List<ProductDTO>) *@
//            @* { *@
//            @*     <option value="@product.Id" data-price="@product.Price">@product.Name</option> *@
//            @* } *@
//        </select>

//    </div>
    
//    <div class="form-group">
//        <label for="price">Price</label>
//        <input type="text" id="price" class="form-control" readonly>
//    </div>
    
//    <div class="form-group">
//        <label for="quantity">Quantity</label>
//        <input type="number" id="quantity" class="form-control" min="1" value="1">
//    </div>
    
//    <div class="form-group">
//        <label for="subtotal">Subtotal</label>
//        <input type="text" id="subtotal" class="form-control" readonly>
//    </div>
    
//    <div class="form-group">
//        <label for="gst">GST (%)</label>
//        <input type="number" id="gst" class="form-control" min="0" value="5">
//    </div>
    
//    <div class="form-group">
//        <label for="discount">Discount (%)</label>
//        <input type="number" id="discount" class="form-control" min="0" value="0">
//    </div>
    
//    <div class="form-group">
//        <label for="total">Total</label>
//        <input type="text" id="total" class="form-control" readonly>
//    </div>
    
//    @* <form asp-action="GenerateInvoice" asp-controller="Billing" method="post"> *@
//    @*     <input type="hidden" asp-for="Id" /> *@
//    @*     <input type="hidden" asp-for="CustomerId" /> *@
//    @*     <input type="hidden" asp-for="TotalAmount" /> *@

//        <button type="submit" class="btn btn-success">Generate Invoice</button>
//    @* </form> *@

//</form>

//<script>
//    document.addEventListener("DOMContentLoaded", function () {
//        const productDropdown = document.getElementById("product");
//        const priceInput = document.getElementById("price");
//        const quantityInput = document.getElementById("quantity");
//        const subtotalInput = document.getElementById("subtotal");
//        const gstInput = document.getElementById("gst");
//        const discountInput = document.getElementById("discount");
//        const totalInput = document.getElementById("total");
        
//        function calculateTotal() {
//            let price = parseFloat(priceInput.value) || 0;
//            let quantity = parseInt(quantityInput.value) || 1;
//            let gst = parseFloat(gstInput.value) || 0;
//            let discount = parseFloat(discountInput.value) || 0;
            
//            let subtotal = price * quantity;
//            let gstAmount = (subtotal * gst) / 100;
//            let discountAmount = (subtotal * discount) / 100;
//            let total = subtotal + gstAmount - discountAmount;
            
//            subtotalInput.value = subtotal.toFixed(2);
//            totalInput.value = total.toFixed(2);
//        }
        
//        productDropdown.addEventListener("change", function () {
//            let selectedOption = productDropdown.options[productDropdown.selectedIndex];
//            let price = selectedOption.getAttribute("data-price");
//            priceInput.value = price;
//            calculateTotal();
//        });
        
//        quantityInput.addEventListener("input", calculateTotal);
//        gstInput.addEventListener("input", calculateTotal);
//        discountInput.addEventListener("input", calculateTotal);
//    });

//    document.getElementById("billingForm").addEventListener("submit", function(event) {
//        console.log("Form submitted!"); // Debugging
//    });
//</script>
