import { useEffect } from 'react';
import { onCLS, onFID, onFCP, onLCP, onTTFB } from 'web-vitals';

function reportWebVitals(metric) {
  console.log(metric);
  
  // Send to analytics service
  if (process.env.NODE_ENV === 'production') {
    // Example: Send to Google Analytics
    // gtag('event', metric.name, {
    //   event_category: 'Web Vitals',
    //   value: Math.round(metric.value),
    //   event_label: metric.id,
    //   non_interaction: true,
    // });
  }
}

export function useWebVitals() {
  useEffect(() => {
    onCLS(reportWebVitals);
    onFID(reportWebVitals);
    onFCP(reportWebVitals);
    onLCP(reportWebVitals);
    onTTFB(reportWebVitals);
  }, []);
}

export default useWebVitals;
