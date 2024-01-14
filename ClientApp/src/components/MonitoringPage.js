import React from 'react';
import { useState, useEffect } from 'react';
import CloudCoverAndCurrentPowerChart from './CloudCoverAndCurrentPowerChart';
import CloudCoverAndCurrentPowerInvertersChart from './CloudCoverAndCurrentPowerInvertersChart';
import CloudCoverAverageAndCurrentPower from './CloudCoverAverageAndCurrentPower';
import DatePicker from 'react-datepicker';
import Select from 'react-select';
import { useNavigate } from 'react-router-dom';
import authService from './api-authorization/AuthorizeService';
import DayProducedEnergyChart from './DayProducedEnergyChart';

const hourDaysLimit = 15;
const dayDaysLimit = 90;

function MonitoringPage() {

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

  // --------------- 2nd chart ---------------
  const [selectedDate, setSelectedDate] = useState(new Date('2016-07-25'));
  const handleSelectedDateChange = (date) => setSelectedDate(date);

  // --------------- 3rd chart ---------------
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

  // --------------- 4th chart ---------------
  const [selectedDate2, setSelectedDate2] = useState(new Date('2016-07-25'));
  const handleSelectedDateChange2 = (date) => setSelectedDate2(date);
  
  // --------------- Page ---------------
  return (
    <div>
      <label>Start Date: </label>
      <DatePicker
        selected={startDate}
        onChange={handleStartDateChange}
        showMonthDropdown
        showYearDropdown
        filterDate={handleFilterStart}
      />
      <label>End Date: </label>
      <DatePicker
        selected={endDate}
        onChange={handleEndDateChange}
        showMonthDropdown
        showYearDropdown
        filterDate={handleFilterEnd}
      />
      <label>Aggregate type: </label>
      <Select
        value={aggregateType}
        options={AggregateTypes(startDate, endDate)}
        onChange={handleAggregateTypeChange}
      />
      <label>Inverter: </label>
      <Select value={inverter} options={allInverters} onChange={handleInverterChange} />
      <button onClick={showInTableButton}>Show in table</button>
      <br></br>
      <CloudCoverAndCurrentPowerChart
        startDate={startDate}
        endDate={endDate}
        aggregateType={aggregateType.value}
        inverter={inverter}
      />
      <label>Date to compare: </label>
      <DatePicker selected={selectedDate} onChange={handleSelectedDateChange} />
      <CloudCoverAverageAndCurrentPower selectedDate={selectedDate} />


      <label>Start Date: </label>
      <DatePicker
        selected={startDate2}
        onChange={handleStartDateChange2}
        showMonthDropdown
        showYearDropdown
        filterDate={handleFilterStart2}
      />
      <label>End Date: </label>
      <DatePicker
        selected={endDate2}
        onChange={handleEndDateChange2}
        showMonthDropdown
        showYearDropdown
        filterDate={handleFilterEnd2}
      />
      <label>Aggregate type: </label>
      <Select
        value={aggregateType2}
        options={AggregateTypes(startDate2, endDate2)}
        onChange={handleAggregateTypeChange2}
      />
      {/* <button onClick={tableButton}>Show in table</button> */}
      <CloudCoverAndCurrentPowerInvertersChart
        startDate={startDate2}
        endDate={endDate2}
        aggregateType={aggregateType2.value}
      />

      <label>Date : </label>
      <DatePicker selected={selectedDate2} onChange={handleSelectedDateChange2} />
      <DayProducedEnergyChart date={selectedDate2} />
    </div>
  );
}

export default MonitoringPage;
