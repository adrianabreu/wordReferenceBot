$(function () {

    $.ajax('https://wrefbot-aabreuglez.rhcloud.com/ddbb/stats')
    .done(function(stats) {

        if(stats) {
            var params = {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Wrefbot usage'
                },
                xAxis: {
                    categories: [ 'Right now' ],
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Translations stored'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.0f}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: [
                {
                    name: 'Eng to Spa',
                    data: [+stats[0].count]
                },
                {
                    name: 'Spa to End',
                    data: [+stats[1].count]
                }
                ]
            }

            var total = stats[0].count + stats[1].count;

            var params2 =  {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Wrefbot usage'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: false
                        },
                        showInLegend: true
                    }
                },
                series: [{
                    name: 'Translations',
                    colorByPoint: true,
                    data: [
                        {
                            name: 'Eng to Spa',
                            y: (+stats[0].count / total)
                        },
                        {
                            name: 'Spa to Eng',
                            y: (+stats[1].count / total)
                        }
                    ]
                }]
            }

            $('#container').highcharts(params);
            $('#container2').highcharts(params2);
        }
    })
});