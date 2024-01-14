import { useState, useEffect } from 'react';
import authService from './api-authorization/AuthorizeService';
import ReactECharts from 'echarts-for-react';
import React from 'react';

function DayProducedEnergyChart(props) {
  const [response, setResponse] = useState(null);
  const [isLoading, setLoading] = useState(true);

  useEffect(() => {
    handleShowClick();
  }, []); // eslint-disable-line react-hooks/exhaustive-deps

  const handleShowClick = () => {
    setLoading(true);
    authService.getAccessToken().then((token) => {
        fetch(
          `monitoring/DayProducedEnergyInverters?date=${encodeURIComponent(
            props.date.toISOString()
          )}`,
          {
            headers: !token ? {} : { Authorization: `Bearer ${token}` },
          }
        ).then((response) => {
          response.json().then((data) => {
            setResponse(data);
            setLoading(false);
          });
        });
      });
  };

  var dataX = response?.date.map((d) => `${new Date(d).getHours()}:${new Date(d).getMinutes()}`);

  function getLegend()
  {
    var legendList = [];
    for ( var j = 0; j < response?.inverters.length; j++)
        {
          var inverter = response?.inverters[j].inverterID;
          legendList.push(inverter);
        }
        // legendList.push('Cloud cover');
        return legendList;
  }

  function getSeries() 
  {
    var seriesList = [];
    for ( var j = 0; j < response?.inverters.length; j++)
        {
          var inverter = response?.inverters[j].inverterID;
          var data = response?.inverters[j].produced;
            seriesList.push(
                          {
                            name: inverter,
                            type: 'line',
                            yAxisIndex: 0,
                            tooltip: {
                              valueFormatter: function (value) {
                                return value + ' kWh';
                              },
                            },
                            data: data,
                          }
                        );
            }

      // seriesList.push({
      //   name: 'Cloud cover',
      //   type: 'line',
      //   yAxisIndex: 0,
      //   tooltip: {
      //     valueFormatter: function (value) {
      //       return value + ' %';
      //     },
      //   },
      //   data: response?.cloudCover,
      // });
      return seriesList;
  }
  
  function getMax()
  {
    var max = 0;
    for ( var j = 0; j < response?.inverters.length; j++)
        {
          var data = response?.inverters[j].produced;
          var localMax = Math.max(...data);
          if (localMax > max) {max = localMax}
        }
    return max;
  }

  const options = {
    tooltip: {
      trigger: 'axis',
      axisPointer: {
        type: 'cross',
        crossStyle: {
          color: '#999',
        },
      },
    },
    toolbox: {
      feature: {
        dataView: { show: true, readOnly: false },
        magicType: { show: true, type: ['line', 'bar'] },
        restore: { show: true },
        saveAsImage: { show: true },
      },
    },
    legend: {
      data: getLegend(),
    },
    xAxis: [
      {
        type: 'category',
        data: dataX,
        axisPointer: {
          type: 'shadow',
        },
      },
    ],
    yAxis: [
      // {
      //   type: 'value',
      //   name: 'Cloud cover',
      //   min: 0,
      //   max: 100,
      //   interval: 25,
      //   axisLabel: {
      //     formatter: '{value} %',
      //   },
      // },
      {
        type: 'value',
        name: 'Energy produced',
        min: 0,
        max: getMax(),
        interval: getMax() / 10,
        axisLabel: {
          formatter: '{value} kWh',
        },
      },
    ],
    series: getSeries(),
  };

  const optionsIfEmpty = {
    graphic: {
      elements: [
        {
          type: 'text',
          left: 'center',
          top: 'middle',
          style: {
            text: 'No Data',
            font: '18px Arial',
            fill: '#999',
          },
          z: 100,
        },
      ],
    },
  };

  return (
    <div>
      <button className='chart-page__button' onClick={handleShowClick} disabled={isLoading === true && response !== null}>Show</button>
      {response && response.date.length === 0 ? (
        <ReactECharts
          option={optionsIfEmpty}
          notMerge={true}
          lazyUpdate={true}
          showLoading={isLoading === true || response === null}
        />
      ) : (
        <ReactECharts
          option={options}
          notMerge={true}
          lazyUpdate={true}
          showLoading={isLoading === true || response === null}
        />
      )}
    </div>
  );
}

export default DayProducedEnergyChart;
