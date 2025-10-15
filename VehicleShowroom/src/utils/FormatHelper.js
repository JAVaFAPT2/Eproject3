export const formatUSD = (value) => {
  if (value === undefined || value === null || isNaN(value)) return '$0.00';
  return `$${Number(value).toLocaleString('en-US', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  })}`;
};

export const formatCompact = (value) => {
  if (value === undefined || value === null || isNaN(value)) return '0';
  return new Intl.NumberFormat('en', { notation: 'compact' }).format(value);
};
