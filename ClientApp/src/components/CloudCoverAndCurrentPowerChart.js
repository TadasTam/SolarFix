import { useState, useEffect } from 'react';
import authService from './api-authorization/AuthorizeService';
import ReactECharts from 'echarts-for-react';
import React from 'react';

function CloudCoverAndCurrentPowerChart(props) {
  const [response, setResponse] = useState(null);
  const [isLoading, setLoading] = useState(true);

  useEffect(() => {
    handleShowClick();
  }, []); // eslint-disable-line react-hooks/exhaustive-deps
  
  const handleShowClick = () => {
    setLoading(true);
    props.inverter &&
      authService.getAccessToken().then((token) => {
        fetch(
          `monitoring/CloudCoverAndCurrentPower?startDate=${encodeURIComponent(
            props.startDate.toISOString()
          )}&endDate=${encodeURIComponent(
            props.endDate.toISOString()
          )}&aggregateType=${encodeURIComponent(props.aggregateType)}&inverter=${encodeURIComponent(
            props.inverter.value
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

  function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
  }

  var dataX = response?.date.map((d) => `${new Date(d).getHours()}:00`);
  if (response?.type === 'Hour') {
    dataX = response?.date.map((d) => `${new Date(d).getHours()}:00`);
  } else if (response?.type === 'Day') {
    dataX = response?.date.map(
      (d) => `${new Date(d).getFullYear()}-${new Date(d).getMonth() + 1}-${new Date(d).getDate()}`
    );
  } else if (response?.type === 'Week') {
    dataX = response?.date.map(
      (d) =>
        `${new Date(d).getFullYear()}-${new Date(d).getMonth() + 1}-${new Date(
          d
        ).getDate()} to ${addDays(new Date(d), 6).getFullYear()}-${
          addDays(new Date(d), 6).getMonth() + 1
        }-${addDays(new Date(d), 6).getDate()}`
    );
  } else if (response?.type === 'Month') {
    dataX = response?.date.map((d) => `${new Date(d).getFullYear()}-${new Date(d).getMonth() + 1}`);
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
      data: ['Total energy', 'Cloud cover'],
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
      {
        type: 'value',
        name: 'Cloud cover',
        min: 0,
        max: 100,
        interval: 25,
        axisLabel: {
          formatter: '{value} %',
        },
      },
      {
        type: 'value',
        name: 'Total energy',
        min: 0,
        max: response && Math.round(Math.max(...response.currentPower)),
        interval: response && Math.round(Math.max(...response.currentPower)) / 4,
        axisLabel: {
          formatter: '{value} kWh',
        },
      },
    ],
    series: [
      {
        name: 'Total energy',
        type: 'bar',
        yAxisIndex: 1,
        tooltip: {
          valueFormatter: function (value) {
            return value + ' kWh';
          },
        },
        data: response?.currentPower,
      },
      {
        name: 'Cloud cover',
        type: 'line',
        yAxisIndex: 0,
        tooltip: {
          valueFormatter: function (value) {
            return value + ' %';
          },
        },
        data: response?.cloudCover,
      },
    ],
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

export default CloudCoverAndCurrentPowerChart;
