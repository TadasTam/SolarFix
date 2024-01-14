import React from 'react';
import { useState } from 'react';
import CloudCoverAndCurrentPowerInvertersChart from './CloudCoverAndCurrentPowerInvertersChart';
import DatePicker from 'react-datepicker';
import Select from 'react-select';
import '../custom.css';


const hourDaysLimit = 15;
const dayDaysLimit = 90;

function CloudCoverAndCurrentPowerInvertersChartPage() {

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

  const [startDate2, setStartDate2] = useState(new Date('2016-07-25'));
  const [endDate2, setEndDate2] = useState(new Date('2016-07-26'));
  const [aggregateType2, setAggregateType2] = useState(AggregateTypes(startDate2, endDate2)[0]);
  
  const handleAggregateTypeChange2 = (value) => setAggregateType2(value);
  const handleFilterStart2 = (date) => filterStart(date, endDate2);
  const handleFilterEnd2 = (date) => filterEnd(date, startDate2);

  const handleStartDateChange2 = (date) => {
    setStartDate2(date);
    const diff = Math.abs(endDate2.getTime() - date.getTime()) / 86400000;

    if ((diff > dayDaysLimit && (aggregateType2.value === 'day' || aggregateType2.value === 'hour')) || (diff > hourDaysLimit && aggregateType2.value === 'hour')) {
      const previous = aggregateType2.value;
      var newest;

      if (diff > dayDaysLimit) {setAggregateType2({ value: 'week', label: 'Week' }); newest = 'week'; } else if (diff > hourDaysLimit) {setAggregateType2({ value: 'day', label: 'Day' }); newest = 'day'; };
      alert('Date interval is too wide for ' + previous + ' aggregate type, changed to ' + newest);
    }
  };
  
  const handleEndDateChange2 = (date) => {
    setEndDate2(date);
    const diff = Math.abs(date.getTime() - startDate2.getTime()) / 86400000;

    if ((diff > dayDaysLimit && (aggregateType2.value === 'day' || aggregateType2.value === 'hour')) || (diff > hourDaysLimit && aggregateType2.value === 'hour')) {
      const previous = aggregateType2.value;
      var newest;

      if (diff > dayDaysLimit) {setAggregateType2({ value: 'week', label: 'Week' }); newest = 'week'; } else if (diff > hourDaysLimit) {setAggregateType2({ value: 'day', label: 'Day' }); newest = 'day'; };
      alert('Date interval is too wide for ' + previous + ' aggregate type, changed to ' + newest);
    }
  };
  
  return (
    <div className="chart-page">
      <div className="chart-page__header">
        <label className="chart-page__label">Start Date:</label>
      <DatePicker
        selected={startDate2}
        onChange={handleStartDateChange2}
        showMonthDropdown
        showYearDropdown
        filterDate={handleFilterStart2}
        className="chart-page__datepicker"
      />
      <label className="chart-page__label">End Date:</label>
      <DatePicker
        selected={endDate2}
        onChange={handleEndDateChange2}
        showMonthDropdown
        showYearDropdown
        filterDate={handleFilterEnd2}
        className="chart-page__datepicker"
      />
      <label className="chart-page__label">Aggregate Type:</label>
      <Select
        value={aggregateType2}
        options={AggregateTypes(startDate2, endDate2)}
        onChange={handleAggregateTypeChange2}
        className="chart-page__select"
      />
      </div>
      <div className="chart-page__chart">
      <CloudCoverAndCurrentPowerInvertersChart
        startDate={startDate2}
        endDate={endDate2}
        aggregateType={aggregateType2.value}
      />
    </div>
    </div>
  );
}

export default CloudCoverAndCurrentPowerInvertersChartPage;
