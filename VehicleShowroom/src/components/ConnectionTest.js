import React, { useState, useEffect } from 'react';
import { Box, Button, Text, VStack, HStack, Badge, Alert, AlertIcon } from '@chakra-ui/react';
import ApiClient from '../api/ApiClient';

const ConnectionTest = () => {
  const [connectionStatus, setConnectionStatus] = useState('unknown');
  const [apiResponse, setApiResponse] = useState(null);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);

  const testConnection = async () => {
    setLoading(true);
    setError(null);
    
    try {
      // Test basic API connection
      const response = await ApiClient.get('/health');
      setConnectionStatus('connected');
      setApiResponse(response.data);
    } catch (err) {
      setConnectionStatus('failed');
      setError(err.message);
      console.error('Connection test failed:', err);
    } finally {
      setLoading(false);
    }
  };

  const testVehicleModels = async () => {
    setLoading(true);
    setError(null);
    
    try {
      // Test vehicle models endpoint
      const response = await ApiClient.get('/VehicleModels');
      setConnectionStatus('connected');
      setApiResponse({
        endpoint: 'VehicleModels',
        count: response.data?.items?.length || 0,
        data: response.data
      });
    } catch (err) {
      setConnectionStatus('failed');
      setError(err.message);
      console.error('Vehicle models test failed:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    // Auto-test on component mount
    testConnection();
  }, []);

  const getStatusColor = () => {
    switch (connectionStatus) {
      case 'connected': return 'green';
      case 'failed': return 'red';
      default: return 'gray';
    }
  };

  const getStatusText = () => {
    switch (connectionStatus) {
      case 'connected': return 'Connected';
      case 'failed': return 'Failed';
      default: return 'Unknown';
    }
  };

  return (
    <Box p={6} maxW="600px" mx="auto">
      <VStack spacing={4} align="stretch">
        <Text fontSize="2xl" fontWeight="bold" textAlign="center">
          Backend Connection Test
        </Text>
        
        <HStack justify="center" spacing={4}>
          <Badge colorScheme={getStatusColor()} fontSize="lg" px={3} py={1}>
            Status: {getStatusText()}
          </Badge>
          <Text fontSize="sm" color="gray.600">
            API URL: {process.env.REACT_APP_API_URL || 'Not set'}
          </Text>
        </HStack>

        {error && (
          <Alert status="error">
            <AlertIcon />
            <Text fontSize="sm">{error}</Text>
          </Alert>
        )}

        <HStack spacing={4} justify="center">
          <Button
            colorScheme="blue"
            onClick={testConnection}
            isLoading={loading}
            loadingText="Testing..."
          >
            Test Health Endpoint
          </Button>
          
          <Button
            colorScheme="green"
            onClick={testVehicleModels}
            isLoading={loading}
            loadingText="Testing..."
          >
            Test Vehicle Models
          </Button>
        </HStack>

        {apiResponse && (
          <Box p={4} bg="gray.50" borderRadius="md">
            <Text fontWeight="bold" mb={2}>API Response:</Text>
            <Text fontSize="sm" fontFamily="mono" whiteSpace="pre-wrap">
              {JSON.stringify(apiResponse, null, 2)}
            </Text>
          </Box>
        )}

        <Box p={4} bg="blue.50" borderRadius="md">
          <Text fontWeight="bold" mb={2}>Connection Details:</Text>
          <VStack align="start" spacing={1}>
            <Text fontSize="sm">
              <strong>Frontend URL:</strong> {window.location.origin}
            </Text>
            <Text fontSize="sm">
              <strong>API Base URL:</strong> {process.env.REACT_APP_API_URL || 'Not configured'}
            </Text>
            <Text fontSize="sm">
              <strong>Environment:</strong> {process.env.REACT_APP_ENVIRONMENT || 'development'}
            </Text>
            <Text fontSize="sm">
              <strong>Timestamp:</strong> {new Date().toLocaleString()}
            </Text>
          </VStack>
        </Box>
      </VStack>
    </Box>
  );
};

export default ConnectionTest;
