google.charts.load('current', {'packages':['bar']});
google.charts.load('current', {'packages':['corechart']});
google.charts.setOnLoadCallback(drawChart);

function drawChart() {
    var data = google.visualization.arrayToDataTable([
        ['Year', 'Sales', 'Expenses', 'Profit'],
        ['2014', 1000, 400, 200],
        ['2015', 1170, 460, 250],
        ['2016', 660, 1120, 300],
        ['2017', 1030, 540, 350]
    ]);

    var options = {
        chart: {
            title: 'Trends',
            subtitle: 'Sales, Expenses, and Profit: 2014-2017',
        },
        width:400,
        height:450
    };

    var chart = new google.charts.Bar(document.getElementById('columnchart_material'));

    chart.draw(data, google.charts.Bar.convertOptions(options));

    var dataDonut1 = google.visualization.arrayToDataTable([
        ['Effort', 'Amount given'],
        ['Total spent',     200],
        ['Money saved',     50]
    ]);

    var optionsDonut1 = {
        pieHole: 0.6,
        pieSliceTextStyle: {
            color: 'black',
        },
        slices: {
            0: {color:'#5199FF'},
            1: {color:'#E5F0FF'}
        },
        width:450,
        height:300,
    };

    var chartDonut1 = new google.visualization.PieChart(document.getElementById('donut_single1'));
    chartDonut1.draw(dataDonut1, optionsDonut1);

    var dataDonut2 = google.visualization.arrayToDataTable([
        ['Effort', 'Amount given'],
        ['Total spent',     200],
        ['Money saved',     50]
    ]);

    var optionsDonut2 = {
        pieHole: 0.6,
        pieSliceTextStyle: {
            color: 'black',
        },
        slices: {
            0: {color:'#5199FF'},
            1: {color:'#E5F0FF'}
        },
        width:450,
        height:300,
    };

    var chartDonut2 = new google.visualization.PieChart(document.getElementById('donut_single2'));
    chartDonut2.draw(dataDonut2, optionsDonut2);
}
google.charts.load('current', {
    'packages': ['geochart'],
    // Note: you will need to get a mapsApiKey for your project.
    // See: https://developers.google.com/chart/interactive/docs/basic_load_libs#load-settings
    'mapsApiKey': 'AIzaSyD-9tSrke72PouQMnMX-a7eZSW0jkFMBWY'
});
google.charts.setOnLoadCallback(drawMarkersMap);

function drawMarkersMap() {
    var data = google.visualization.arrayToDataTable([
        ['Country',   'Users', 'Area Percentage'],
        ['France',  65700000, 23],
        ['Germany', 81890000, 27],
        ['Poland',  38540000, 23],
        ['Russia',  112990222, 50]
    ]);

    var options = {
        width:550,
        height:450,
        sizeAxis: { minValue: 0, maxValue: 100 },
        // displayMode: 'markers',
        colorAxis: {colors: ['#e7711c', '#4374e0']} // orange to blue
    };

    var chart = new google.visualization.GeoChart(document.getElementById('chart_div'));
    chart.draw(data, options);
};
google.charts.load('current', {packages: ['corechart', 'bar']});
google.charts.setOnLoadCallback(drawBasic);

function drawBasic() {

    var data = google.visualization.arrayToDataTable([
        ['Country', 'Users'],
        ['France',  65700000],
        ['Germany', 81890000],
        ['Poland',  38540000],
        ['Russia',  112990222]
    ]);

    var options = {
        title: 'Users by countries',
        chartArea: {width: '60%'},
        hAxis: {
            minValue: 0
        },
        vAxis: {
        },
        width:450,
        height:450,
    };

    var chart = new google.visualization.BarChart(document.getElementById('chart_div1'));

    chart.draw(data, options);
}