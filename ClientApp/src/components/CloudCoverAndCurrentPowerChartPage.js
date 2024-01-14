import React from 'react';
import { useState, useEffect } from 'react';
import CloudCoverAndCurrentPowerChart from './CloudCoverAndCurrentPowerChart';
import DatePicker from 'react-datepicker';
import Select from 'react-select';
import { useNavigate } from 'react-router-dom';
import authService from './api-authorization/AuthorizeService';
import '../custom.css';

const hourDaysLimit = 15;
const dayDaysLimit = 90;

function CloudCoverAndCurrentPowerChartPage() {

  // --------------- 1st chart ---------------
  const [startDate, setStartDate] = useState(new Date('2016-07-25'));
  const [endDate, setEndDate] = useState(new Date('2016-07-26'));
  const [aggregateType, setAggregateType] = useState(AggregateTypes(startDate, endDate)[0]);
  const [inverter, setInverter] = useState();
  const [allInverters, setAllInverters] = useState();

  useEffect(() => {
    authService.getAccessToken().then((token) => {
      fetch(`inverter/GetAll`, {
        headers: !token ? {} : { Authorization: `Bearer ${token}` },
      }).then((response) => {
        response.json().then((data) => {
          setAllInverters(data);
          setInverter(data[0]);
        });
      });
    });
  }, []);

  const handleAggregateTypeChange = (value) => setAggregateType(value);
  const handleFilterStart = (date) => filterStart(date, endDate);
  const handleFilterEnd = (date) => filterEnd(date, startDate);
  const handleInverterChange = (value) => setInverter(value);

  const handleStartDateChange = (date) => {
    setStartDate(date);
    const diff = Math.abs(endDate.getTime() - date.getTime()) / 86400000;

    if ((diff > dayDaysLimit && (aggregateType.value === 'day' || aggregateType.value === 'hour')) || (diff > hourDaysLimit && aggregateType.value === 'hour')) {
      const previous = aggregateType.value;
      var newest;

      if (diff > dayDaysLimit) {setAggregateType({ value: 'week', label: 'Week' }); newest = 'week'; } else if (diff > hourDaysLimit) {setAggregateType({ value: 'day', label: 'Day' }); newest = 'day'; };
      alert('Date interval is too wide for ' + previous + ' aggregate type, changed to ' + newest);
    }
  };
  
  const handleEndDateChange = (date) => {
    setEndDate(date);
    const diff = Math.abs(date.getTime() - startDate.getTime()) / 86400000;

    if ((diff > dayDaysLimit && (aggregateType.value === 'day' || aggregateType.value === 'hour')) || (diff > hourDaysLimit && aggregateType.value === 'hour')) {
      const previous = aggregateType.value;
      var newest;

      if (diff > dayDaysLimit) {setAggregateType({ value: 'week', label: 'Week' }); newest = 'week'; } else if (diff > hourDaysLimit) {setAggregateType({ value: 'day', label: 'Day' }); newest = 'day'; };
      alert('Date interval is too wide for ' + previous + ' aggregate type, changed to ' + newest);
    }
  };
  
  function AggregateTypes(start, end) {
    const diff = Math.abs(end - start) / 86400000;
    if (diff > dayDaysLimit) {
      return [
        { value: 'week', label: 'Week' },
        { value: 'month', label: 'Month' },
      ];
    } else if (diff > hourDaysLimit) {
      return [
        { value: 'day', label: 'Day' },
        { value: 'week', label: 'Week' },
        { value: 'month', label: 'Month' },
      ];
    } else {
      return [
        { value: 'hour', label: 'Hour' },
        { value: 'day', label: 'Day' },
        { value: 'week', label: 'Week' },
        { value: 'month', label: 'Month' },
      ];
    }
  }

  function filterStart(date, end) { return date <= new Date() && date <= end; }
  function filterEnd(date, start) { return date <= new Date() && date >= addDays(start, -1); }
  function addDays(date, days) { return (new Date(date)).setDate(date.getDate() + days) }

  const navigate = useNavigate();
  const showInTableButton = () => {
    navigate(`/InverterDataTable`, {
      state: {
        startDate: startDate,
        endDate: endDate,
      },
    });
  };

  return (
    <div className="chart-page">
      <div className="chart-page__header">
      <label className="chart-page__label">Start Date:</label>
      <DatePicker
        selected={startDate}
        onChange={handleStartDateChange}
        showMonthDropdown
        showYearDropdown
        filterDate={handleFilterStart}
        className="chart-page__datepicker"
      />
      <label className="chart-page__label">End Date: </label>
      <DatePicker
        selected={endDate}
        onChange={handleEndDateChange}
        showMonthDropdown
        showYearDropdown
        filterDate={handleFilterEnd}
        className="chart-page__datepicker"
      />
      <label className="chart-page__label">Aggregate type: </label>
      <Select
        value={aggregateType}
        options={AggregateTypes(startDate, endDate)}
        onChange={handleAggregateTypeChange}
        className="chart-page__select"
      />
      <label className="chart-page__label">Inverter: </label>
      <Select value={inverter} options={allInverters} onChange={handleInverterChange} className="chart-page__select"/>
      </div>
      <div className="chart-page__chart">
        <button className='chart-page__button' onClick={showInTableButton}>Show in table</button>
      <CloudCoverAndCurrentPowerChart
        startDate={startDate}
        endDate={endDate}
        aggregateType={aggregateType.value}
        inverter={inverter}
      />
      </div>
    </div>
  );
}

export default CloudCoverAndCurrentPowerChartPage;
