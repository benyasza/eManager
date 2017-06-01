$('#btnJob').on('click', function () {
    event.preventDefault();

    $nextId = Math.floor(Math.random() * 9999) + 1000;

    $row = $(this).closest('tbody').find('tr:nth-last-child(2)');
    $newRow = $row.clone();

    $hairDressers = $newRow.find('td:first>select');
    $hairDressers.attr('id', 'hairdressers_' + $nextId);
    $hairDressers.attr('name', 'hairdressers_' + $nextId);

    $jobs = $newRow.find('td:nth-child(2)>select');
    $jobs.attr('id', 'jobs_' + $nextId);
    $jobs.attr('name', 'jobs_' + $nextId);

    $newRow.find('td:nth-last-child(3) > input').val(0);
    $newRow.find('td:nth-last-child(2)').text('');

    $newRow.insertAfter($row);
});

$('#btnMaterial').on('click', function () {
    event.preventDefault();

    $nextId = Math.floor(Math.random() * 9999) + 1000;

    $row = $(this).closest('tbody').find('tr:nth-last-child(2)');
    $newRow = $row.clone();

    $materials = $newRow.find('td:first>select');
    $materials.attr('id', 'materials_' + $nextId);
    $materials.attr('name', 'materials_' + $nextId);

    $newRow.find('td:nth-last-child(2)').text('');
    $newRow.find('td:nth-child(2) > input').val(0);

    $newRow.insertAfter($row);
});

$('#btnProduct').on('click', function () {
    event.preventDefault();

    $nextId = Math.floor(Math.random() * 9999) + 1000;

    $row = $(this).closest('tbody').find('tr:nth-last-child(2)');
    $newRow = $row.clone();

    $materials = $newRow.find('td:first>select');
    $materials.attr('id', 'products_' + $nextId);
    $materials.attr('name', 'products_' + $nextId);

    $newRow.find('td:nth-last-child(2)').text('');
    $newRow.find('td:nth-child(2) > input').val(0);

    $newRow.insertAfter($row);
});

$('div.invoices a.delete').on('click', removeRow);

var removeRow = function ($this) {
    event.preventDefault();

    $row = $this.closest('tr');
    $priceCell = $row.find('td:nth-last-child(2)');

    $rowCount = $this.closest('table > tbody').find('tr').length;

    if ($rowCount > 2) {
        updateTotalPrice($priceCell.text(), 0);
        $row.remove();
    }
};

var updateTablePrices = function ($select) {
    event.preventDefault();

    // update prices
    $row = $select.closest('tr');
    $priceCell = $row.find('td:nth-last-child(2)');

    $table = $select.closest('table');

    if ($table.hasClass('jobs')) {
        $row.find('td:nth-last-child(3) > input').val(0);
    }

    if ($table.hasClass('materials') || $table.hasClass('products')) {
        if ($select[0].value != '') {
            $row.find('td:nth-child(2) > input').val(1);
        } else {
            $row.find('td:nth-child(2) > input').val(0);
        }
    }

    $oldPrice = $priceCell.text();
    $newPrice = $select.children(':selected').data('cost');

    $priceCell.text($newPrice);
    updateTotalPrice($oldPrice, $newPrice);
};

var updatePricePerDiscount = function ($this) {
    event.preventDefault;

    $priceCell = $this.closest('tr').find('td:nth-last-child(2)');
    $oldPrice = $priceCell.text();

    $select = $this.closest('tr').find('td:nth-child(2) > select');
    $jobPrice = $select.children(':selected').data('cost');

    $discount = Math.min(Math.max($this[0].value, 0), 100);

    if ($oldPrice != '' && $oldPrice != undefined) {
        $newPrice = $jobPrice * (100 - $discount) / 100;

        $priceCell.text($newPrice);
        updateTotalPrice($oldPrice, $newPrice);
    }
};

var updatePricePerQuantity = function ($this) {
    event.preventDefault;

    $priceCell = $this.closest('tr').find('td:nth-last-child(2)');
    $oldPrice = $priceCell.text();

    $select = $this.closest('tr').find('td:nth-child(1) > select');
    $unitPrice = $select.children(':selected').data('cost');

    $quantity = Math.max($this[0].value, 0);

    if ($oldPrice != '' && $oldPrice != undefined) {
        $newPrice = $unitPrice * $quantity;

        $priceCell.text($newPrice);
        updateTotalPrice($oldPrice, $newPrice);
    }
};

var updateTotalPrice = function ($oldValue, $newValue) {
    event.preventDefault();

    $total = $('label#totalCost').text();
    $total = $total - $oldValue + $newValue;
    $('label#totalCost').text($total);
};

$('div.invoices #btnSubmit').on('click', function () {
    event.preventDefault();

    $table = $(this).find('table.jobs');

    // create jobs collection
    var $jobs = [];

    $('table.jobs tr.data').each(function (index, tr) {

        var $data = new Object();
        $data.hairDresserId = $(tr).find('td:nth-child(1) > select')[0].value;
        $data.jobId = $(tr).find('td:nth-child(2) > select')[0].value;
        $data.discount = $(tr).find('td:nth-child(3) > input')[0].value;

        if ($data.hairDresser != '' && $data.job != '' && $data.discount != '') {
            $jobs.push($data);
        }
    });

    // create materials collection
    var $materials = [];

    $('table.materials tr.data').each(function (index, tr) {

        var $data = new Object();
        $data.id = $(tr).find('td:nth-child(1) > select')[0].value;
        $data.quantity = $(tr).find('td:nth-child(2) > input')[0].value;

        if ($data.material != '' && $data.quantity != '') {
            $materials.push($data);
        }
    });

    // create products collection
    var $products = [];

    $('table.products tr.data').each(function (index, tr) {

        var $data = new Object();
        $data.id = $(tr).find('td:nth-child(1) > select')[0].value;
        $data.quantity = $(tr).find('td:nth-child(2) > input')[0].value;

        if ($data.product != '' && $data.quantity != '') {
            $products.push($data);
        }
    });

    // create data object
    $formData = new Object();
    $formData.jobs = $jobs;
    $formData.materials = $materials;
    $formData.products = $products;
    $formData.client = $(document).find('input#Client')[0].value;

    // post data object
    if ($formData != null && $formData != undefined) {

        $.ajax({
            type: "POST",
            url: '/Invoices/Create',
            data: JSON.stringify($formData),
            success: function (data) { console.log('invoice has been created!'); window.location.href = data; },
            contentType: 'application/json',
            dataType: 'json'
        });

    }
});