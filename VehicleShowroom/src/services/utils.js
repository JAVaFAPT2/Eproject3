export const simulateDelay = (result, delay = 300) =>
  new Promise((resolve) => setTimeout(() => resolve(result), delay));

export const simulateReject = (error, delay = 300) =>
  new Promise((_, reject) => setTimeout(() => reject(error), delay));
