import { Component, Inject } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Chart } from 'chart.js';
import * as ChartAnnotation from 'chartjs-plugin-annotation';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {


  chart = [];

  forecast: ForecastResult;

  lumpSum: number = 10000000;
  timescale: number = 5;
  riskLevel: string = 'Low';
  monthlyInvestment: number = 5000;
  targetValue: number = 10300000;

  profileForm = new FormGroup({
    lumpSum: new FormControl(this.lumpSum),
    timescale: new FormControl(this.timescale),
    riskLevel: new FormControl(this.riskLevel),
    monthlyInvestment: new FormControl(this.monthlyInvestment),
    targetValue: new FormControl(this.targetValue)
  });

  client: HttpClient;
  url: string;



  loadGraph() {



    this.client.get<ForecastResult>(

      // TODO: Find a better was of converting the form values to query string for API request

      this.url + 'api/forecast/Projection?TimescaleYears=' + this.timescale + '&MonthlyInvestment=' + this.monthlyInvestment + '&LumpSumInvestment=' + this.lumpSum + '&RiskLevel=' + this.riskLevel + '&TargetValue=' + this.targetValue)
        .subscribe(result => {
      this.forecast = result;

      let dates = this.forecast.dataPoints.map(res => res.date);
      let wideBoundsUpper = this.forecast.dataPoints.map(res => res.wideBandValue.upper);
      let wideBoundsLower = this.forecast.dataPoints.map(res => res.wideBandValue.lower);

      let narrowBoundsUpper = this.forecast.dataPoints.map(res => res.narrowBandValue.upper);
      let narrowBoundsLower = this.forecast.dataPoints.map(res => res.narrowBandValue.lower);

      let totalInvested = this.forecast.dataPoints.map(res => res.totalInvested);
      let targetValues = this.forecast.dataPoints.map(res => res.targetValue);

      // Hack to get around reloading the graph over the top of an existing one
      document.getElementById("chartContainer").innerHTML = '&nbsp;';
      document.getElementById("chartContainer").innerHTML = '<canvas id="canvas">{{ chart }}</canvas>';

      let namedChartAnnotation = ChartAnnotation;

      namedChartAnnotation["id"] = "annotation";
      Chart.pluginService.register(namedChartAnnotation);

      this.chart = new Chart('canvas', {
        type: 'line',
        data: {
          labels: dates,

          datasets: [

            {
              label: 'Total Invested',
              fill: false,
              backgroundColor: "#B22222",
              borderColor: "#B22222",
              lineTension: 0,
              data: totalInvested,
              borderWidth: 3,
              pointRadius: 0
            },

            {
              label: 'Target Value',
              fill: false,
              lineTension: 0,
              backgroundColor: "#000000",
              borderColor:"#000000",
              data: targetValues,
              borderWidth: 3,
              pointRadius: 0
            },

            {
              label: 'Narrow Bounds Upper',
              fill: 3,
              lineTension: 0,
              data: narrowBoundsUpper,
              backgroundColor: "#80ced6",
              borderColor:"#80ced6",
              borderWidth: 3,
              pointRadius: 0
            },

            {
              label: 'Narrow Bounds Lower',
              fill: false,
              lineTension: 0,
              data: narrowBoundsLower,
              backgroundColor: "#80ced6",
              borderColor: "#80ced6",
              borderWidth: 3,
              pointRadius: 0
            },


            {
              label: 'Wide Bounds Upper',
              fill: 5,
              lineTension: 0,
              backgroundColor: "#618685",
              borderColor: "#618685",
              data: wideBoundsUpper,
              borderWidth: 3,
              pointRadius: 0
            },

            {
              label: 'Wide Bounds Lower',
              fill: false,
              lineTension: 0,
              backgroundColor: "#618685",
              borderColor: "#618685",
              data: wideBoundsLower,
              borderWidth: 3,
              pointRadius: 0
            }
          ]
        },
        options: {
          plugins: {
            filler: {
              propagate: true
            }
          },
          scales: {
            xAxes: [{
              type: 'time',
              time: {
                unit: 'year'
              }
            }],
            yAxes: [{
              labelString: '£',
              ticks: {
                beginAtZero: false
              }
            }]
          }
        }
      });


    }, error => console.error(error));
  }

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    this.client = http;
    this.url = baseUrl;

    this.loadGraph();

  }

  onSubmit() {


    this.timescale = this.profileForm.value['timescale'];
    this.lumpSum = this.profileForm.value['lumpSum'];

    
    this.riskLevel = this.profileForm.value['riskLevel'];
    this.monthlyInvestment = this.profileForm.value['monthlyInvestment'];
    this.targetValue = this.profileForm.value['targetValue'];

     this.loadGraph();
  }

}

interface IBandValues {
  upper: number;
  lower: number;
}

interface ForecastResult {
  dataPoints: Array<DataPoint>;
}

interface DataPoint {
  date: string;
  totalInvested: number;
  narrowBandValue: IBandValues;
  wideBandValue: IBandValues;
  targetValue: number;
}
