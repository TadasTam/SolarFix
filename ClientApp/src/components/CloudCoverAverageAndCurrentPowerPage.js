import React from 'react';
import { useState } from 'react';
import CloudCoverAverageAndCurrentPower from './CloudCoverAverageAndCurrentPower';
import DatePicker from 'react-datepicker';

function CloudCoverAverageAndCurrentPowerPage() {
  const [selectedDate, setSelectedDate] = useState(new Date('2016-07-25'));
  const handleSelectedDateChange = (date) => setSelectedDate(date);
  

  return (
    <div className="chart-page">
      <div className="chart-page__header">
      <label className="chart-page__label">Date to compare: </label>
      <DatePicker className="chart-page__datepicker" selected={selectedDate} onChange={handleSelectedDateChange} />
      </div>
      <div className="chart-page__chart">
      <CloudCoverAverageAndCurrentPower selectedDate={selectedDate} />
      </div>
    </div>
  );
}

export default CloudCoverAverageAndCurrentPowerPage;