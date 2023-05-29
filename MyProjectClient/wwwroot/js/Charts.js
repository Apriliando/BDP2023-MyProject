$(function () {
    var femaleEmpArr = []; var maleEmpArr = []; var empPerDept = {};
    //var femaleEmployees = 0; var maleEmployees = 0;  var deptArray = []; 
    $.ajax({
        type: "GET",
        url: "http://localhost:8081/api/Employees",
        dataType: "JSON",
        dataSrc: "data",
        headers: { "Authorization": "Bearer " + sessionStorage.getItem("jwt") }, // 23 - 5 - 2023
    }).done(function (response) {
        response.data.forEach(employee => {
            if (Object.keys(empPerDept).indexOf(employee.department.name) < 0)
                empPerDept[employee.department.name] = 1;
            //deptArray.push(employee.department.name);
            else
                empPerDept[employee.department.name] += 1;

            if (employee.gender == 0)
                maleEmpArr.push(employee);
            else
                femaleEmpArr.push(employee);
        });
        console.log(maleEmpArr, femaleEmpArr, empPerDept);
        //PIE CHART
        var pieChartCanvas = $('#pieChart').get(0).getContext('2d');
        var pieData = {
            labels: [
                'Female',
                'Male',
            ],
            datasets: [
                {
                    data: [femaleEmpArr.length, maleEmpArr.length],
                    backgroundColor: ['pink', 'cyan'],
                    //backgroundColor : ['#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc', '#d2d6de'],
                }
            ]
        };
        //var pieData = donutData;
        var pieOptions = {
            maintainAspectRatio: false,
            responsive: true,
        };
        //Create pie or douhnut chart
        // You can switch between pie and douhnut using the method below.
        new Chart(pieChartCanvas, {
            type: 'pie',
            data: pieData,
            options: pieOptions
        });
        //BAR CHART
        var barChartCanvas = $('#barChart').get(0).getContext('2d')
        var barChartData = {
            labels: Object.keys(empPerDept),
            datasets: [
                {
                    label: 'Number of Employees',
                    backgroundColor: 'rgba(60,141,188,0.9)',
                    borderColor: 'rgba(60,141,188,0.8)',
                    pointRadius: false,
                    pointColor: '#3b8bba',
                    pointStrokeColor: 'rgba(60,141,188,1)',
                    pointHighlightFill: '#fff',
                    pointHighlightStroke: 'rgba(60,141,188,1)',
                    data: Object.values(empPerDept)
                }
            ]
        };
        var barChartOptions = {
            responsive: true,
            maintainAspectRatio: false,
            datasetFill: false,
            scales: {
                yAxes: [{
                    offset: true
                }]
            }
        };
        new Chart(barChartCanvas, {
            type: 'bar',
            data: barChartData,
            options: barChartOptions
        });
        swal({
            title: response.message,
            icon: "success"
        });
    }).fail(function (response) {
        console.log(response);
        //alert(response.message);
        swal({
            title: response.status,
            text: response.statusText,
            icon: "error"
        })
        .then((errorAlert) => {
            if (errorAlert)
                location.replace("/Home/Login");
            else
                location.replace("/Home/Login");
        })
    }).always(function (response) {
        
    });

    //function setFemaleEmpNum(femaleEmpArr) {
    //    femaleEmployees = femaleEmpArr.length;
    //    console.log("inside", femaleEmpArr);
    //};
    //function setMaleEmpNum(maleEmpArr) {
    //    maleEmployees = maleEmpArr.length;
    //    console.log("inside ", maleEmpArr);
    //};

    /* ChartJS
    * -------
    * Here we will create a few charts using ChartJS
    */

    //--------------
    //- AREA CHART -
    //--------------

    // Get context with jQuery - using jQuery's .get() method.
    var areaChartCanvas = $('#areaChart').get(0).getContext('2d')

    var areaChartData = {
        labels  : ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
        datasets: [
            {
                label               : 'Digital Goods',
                backgroundColor     : 'rgba(60,141,188,0.9)',
                borderColor         : 'rgba(60,141,188,0.8)',
                pointRadius          : false,
                pointColor          : '#3b8bba',
                pointStrokeColor    : 'rgba(60,141,188,1)',
                pointHighlightFill  : '#fff',
                pointHighlightStroke: 'rgba(60,141,188,1)',
                data                : [28, 48, 40, 19, 86, 27, 90]
            },
            {
                label               : 'Electronics',
                backgroundColor     : 'rgba(210, 214, 222, 1)',
                borderColor         : 'rgba(210, 214, 222, 1)',
                pointRadius         : false,
                pointColor          : 'rgba(210, 214, 222, 1)',
                pointStrokeColor    : '#c1c7d1',
                pointHighlightFill  : '#fff',
                pointHighlightStroke: 'rgba(220,220,220,1)',
                data                : [65, 59, 80, 81, 56, 55, 40]
            },
        ]
    }

    var areaChartOptions = {
        maintainAspectRatio : false,
        responsive : true,
        legend: {
            display: false
        },
        scales: {
            xAxes: [{
                gridLines : {
                    display : false,
                }
            }],
            yAxes: [{
                gridLines : {
                    display : false,
                }
            }]
        }
    }

    // This will get the first returned node in the jQuery collection.
    new Chart(areaChartCanvas, {
        type: 'line',
        data: areaChartData,
        options: areaChartOptions
    })

    //-------------
    //- LINE CHART -
    //--------------
    var lineChartCanvas = $('#lineChart').get(0).getContext('2d')
    var lineChartOptions = $.extend(true, { }, areaChartOptions)
    var lineChartData = $.extend(true, { }, areaChartData)
    lineChartData.datasets[0].fill = false;
    lineChartData.datasets[1].fill = false;
    lineChartOptions.datasetFill = false

    var lineChart = new Chart(lineChartCanvas, {
        type: 'line',
        data: lineChartData,
        options: lineChartOptions
    })

    //-------------
    //- DONUT CHART -
    //-------------
    // Get context with jQuery - using jQuery's .get() method.
    var donutChartCanvas = $('#donutChart').get(0).getContext('2d')
    var donutData        = {
        labels: [
            'Female',
            'Male',
        ],
        datasets: [
            {
                data: [femaleEmpArr.length, maleEmpArr.length],
                backgroundColor : ['pink', 'cyan'],
                //backgroundColor : ['#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc', '#d2d6de'],
            }
        ]
    }
    var donutOptions     = {
        maintainAspectRatio : false,
        responsive : true,
    }
    //Create pie or douhnut chart
    // You can switch between pie and douhnut using the method below.
    new Chart(donutChartCanvas, {
        type: 'doughnut',
        data: donutData,
        options: donutOptions
    })

    //-------------
    //- PIE CHART -
    //-------------
    // Get context with jQuery - using jQuery's .get() method.
    //var pieChartCanvas = $('#pieChart').get(0).getContext('2d')
    //var pieData        = donutData;
    //var pieOptions     = {
    //    maintainAspectRatio : false,
    //    responsive : true,
    //}
    //Create pie or douhnut chart
    // You can switch between pie and douhnut using the method below.
    //new Chart(pieChartCanvas, {
    //    type: 'pie',
    //    data: pieData,
    //    options: pieOptions
    //})

    //-------------
    //- BAR CHART -
    //-------------
    //var barChartCanvas = $('#barChart').get(0).getContext('2d')
    //var barChartData = $.extend(true, { }, areaChartData)
    //var temp0 = areaChartData.datasets[0]
    //var temp1 = areaChartData.datasets[1]
    //barChartData.datasets[0] = temp1
    //barChartData.datasets[1] = temp0

    //var barChartOptions = {
    //    responsive              : true,
    //    maintainAspectRatio     : false,
    //    datasetFill             : false
    //}

    //new Chart(barChartCanvas, {
    //    type: 'bar',
    //    data: barChartData,
    //    options: barChartOptions
    //})

    //---------------------
    //- STACKED BAR CHART -
    //---------------------
    var stackedBarChartCanvas = $('#stackedBarChart').get(0).getContext('2d')
    var stackedBarChartData = $.extend(true, {}, areaChartData)

    var stackedBarChartOptions = {
        responsive              : true,
        maintainAspectRatio     : false,
        scales: {
            xAxes: [{
            stacked: true,
        }],
        yAxes: [{
            stacked: true
        }]
        }
    }

    new Chart(stackedBarChartCanvas, {
        type: 'bar',
        data: stackedBarChartData,
        options: stackedBarChartOptions
    })
})