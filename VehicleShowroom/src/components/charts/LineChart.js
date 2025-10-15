import React from 'react';
import ReactApexChart from 'react-apexcharts';

const LineChart = ({ series, options }) => {
  return (
    <ReactApexChart
      options={options}
      series={series}
      type="line"
      width="100%"
      height="260"
    />
  );
};

export default LineChart;
