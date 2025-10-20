import React, { useEffect, useState } from 'react';
import {
  Box,
  Image,
  Text,
  VStack,
  Button,
  Flex,
  HStack,
  useDisclosure,
  Spinner,
  Skeleton,
  SkeletonText,
  SkeletonCircle,
  Overlay,
  Modal,
  ModalOverlay,
  ModalContent,
  ModalBody,
} from '@chakra-ui/react';
import { useParams, useNavigate } from 'react-router-dom';
import VehicleModelService from 'services/VehicleModelService';
import VehiclePhotoService from 'services/VehiclePhotoService';
import VehicleSpecService from 'services/VehicleSpecService';
import VSpec from 'views/user/detail/components/VSpec';
import TechnicalDrawer from 'views/user/detail/components/TechnicalDrawer';
import PurchaseDrawer from 'views/user/detail/components/PurchaseDrawer';
import { useAppToast } from 'utils/ToastHelper';
import { useUser } from 'contexts/UserContext'; // ✅ import từ context

export default function Detail() {
  const { slug } = useParams();
  const [model, setModel] = useState(null);
  const [photos, setPhotos] = useState([]);
  const [specs, setSpecs] = useState([]);
  const [loading, setLoading] = useState(true);
  const [loadingPhotos, setLoadingPhotos] = useState(true);
  const [loadingSpecs, setLoadingSpecs] = useState(true);

  const [isTechOpen, setTechOpen] = useState(false);
  const { isOpen, onOpen, onClose } = useDisclosure();
  const navigate = useNavigate();
  const toast = useAppToast();

  // ✅ Lấy user context
  const { isAuthenticated, loading: userLoading } = useUser();

  useEffect(() => {
    let isMounted = true;
    
    async function loadModelData() {
      if (!slug) return;
      
      // Ensure loading states are visible immediately
      setLoading(true);
      setLoadingPhotos(true);
      setLoadingSpecs(true);
      
      // Small delay to ensure loading state is visible
      await new Promise(resolve => setTimeout(resolve, 100));
      
      try {
        // Load model data first
        const data = await VehicleModelService.getBySlug(slug);
        if (isMounted) {
          setModel(data);
          setLoading(false);
        }

        // Load photos and specs in parallel
        const loadPhotos = async () => {
          try {
            const photosRes = await VehiclePhotoService.getByModelNumber(
              data.modelNumber,
            );
            if (isMounted) {
              setPhotos(photosRes?.items || photosRes || []);
            }
          } catch (err) {
            console.error('Failed to load photos:', err);
            console.error('Photos error details:', {
              modelNumber: data.modelNumber,
              error: err.message,
              status: err.response?.status,
              url: err.config?.url
            });
            
            // Handle 404 specifically - photos might not exist for this model
            if (err.response?.status === 404) {
              console.warn(`No photos found for model ${data.modelNumber}`);
              if (isMounted) setPhotos([]);
            } else {
              // For other errors, still set empty array but log the error
              if (isMounted) setPhotos([]);
            }
          } finally {
            if (isMounted) setLoadingPhotos(false);
          }
        };

        const loadSpecs = async () => {
          try {
            const specsRes = await VehicleSpecService.getByModelNumber(
              data.modelNumber,
            );
            if (isMounted) {
              setSpecs(specsRes?.items || specsRes || []);
            }
          } catch (err) {
            console.error('Failed to load specs:', err);
            console.error('Specs error details:', {
              modelNumber: data.modelNumber,
              error: err.message,
              status: err.response?.status,
              url: err.config?.url
            });
            
            // Handle 404 specifically - specs might not exist for this model
            if (err.response?.status === 404) {
              console.warn(`No specs found for model ${data.modelNumber}`);
              if (isMounted) setSpecs([]);
            } else {
              // For other errors, still set empty array but log the error
              if (isMounted) setSpecs([]);
            }
          } finally {
            if (isMounted) setLoadingSpecs(false);
          }
        };

        // Load photos and specs in parallel with timeout
        const timeoutPromise = new Promise((_, reject) => 
          setTimeout(() => reject(new Error('Request timeout')), 10000)
        );
        
        try {
          await Promise.race([
            Promise.all([loadPhotos(), loadSpecs()]),
            timeoutPromise
          ]);
        } catch (timeoutErr) {
          console.warn('Loading timeout reached, forcing completion');
          if (isMounted) {
            setLoadingPhotos(false);
            setLoadingSpecs(false);
          }
        }

      } catch (err) {
        console.error('Failed to load vehicle model detail:', err);
        console.error('Error details:', {
          slug,
          error: err.message,
          response: err.response?.data,
          status: err.response?.status,
        });
        toast.error('Failed to load vehicle model detail');
        
        // Set empty arrays to prevent undefined errors
        if (isMounted) {
          setPhotos([]);
          setSpecs([]);
          setLoading(false);
          setLoadingPhotos(false);
          setLoadingSpecs(false);
        }
      }
    }

    loadModelData();
    
    // Cleanup function
    return () => {
      isMounted = false;
    };
  }, [slug]);

  // 🔹 Ảnh chính và ảnh phụ - Move photo processing before useEffect
  
  // Filter out placeholder URLs and only use real vehicle images
  const isValidPhotoUrl = (url) => {
    if (!url) return false;
    
    // More comprehensive placeholder detection
    const placeholderPatterns = [
      'placehold.co',
      'placeholder',
      'no-image',
      'noimage',
      'via.placeholder.com',
      'dummyimage.com',
      'text=No+Image',
      'text=No Image',
      'text=placeholder',
      'localhost:3000/placeholder',
      'placeholder-car.png',
      '600x400?text=No+Image', // Specific placeholder pattern from network tab
      '600x400?text=No Image'
    ];
    
    // Check for placeholder patterns
    const hasPlaceholderPattern = placeholderPatterns.some(pattern => url.toLowerCase().includes(pattern.toLowerCase()));
    
    // Check for real image patterns (positive validation)
    const realImagePatterns = [
      'cloudinary.com',
      'amazonaws.com',
      'googleapis.com',
      'res.cloudinary.com',
      '.jpg',
      '.jpeg',
      '.png',
      '.webp',
      '.avif',
      'eproject3.onrender.com', // Our API domain
      'onrender.com' // Our hosting domain
    ];
    const hasRealImagePattern = realImagePatterns.some(pattern => url.toLowerCase().includes(pattern.toLowerCase()));
    
    // Allow URLs that have real image patterns OR are from our domain (unless they're clearly placeholders)
    const isValid = !hasPlaceholderPattern && (hasRealImagePattern || url.includes('onrender.com'));
    
    // Debug logging
    console.log('Image URL validation:', {
      url,
      hasPlaceholder: hasPlaceholderPattern,
      hasRealImage: hasRealImagePattern,
      isValid,
      isFromOurDomain: url.includes('onrender.com')
    });
    
    return isValid;
  };
  
  // Filter photos to only include real images
  const realPhotos = photos?.filter(p => isValidPhotoUrl(p.photoUrl)) || [];
  
  // Only use actual photos, no placeholder fallback
  const mainPhoto = realPhotos.find((p) => p.displayOrder === 0)?.photoUrl || realPhotos[0]?.photoUrl;
  const sidePhotos = realPhotos.slice(1) || [];
  
  // Fallback: If no real photos found, try to use any photo that's not clearly a placeholder
  const fallbackPhotos = photos?.filter(p => {
    if (!p.photoUrl) return false;
    // Only exclude obvious placeholders, be more permissive
    const obviousPlaceholders = [
      'text=No+Image',
      'text=No Image',
      '600x400?text=No+Image',
      '600x400?text=No Image'
    ];
    return !obviousPlaceholders.some(pattern => p.photoUrl.toLowerCase().includes(pattern.toLowerCase()));
  }) || [];
  
  const fallbackMainPhoto = fallbackPhotos.find((p) => p.displayOrder === 0)?.photoUrl || fallbackPhotos[0]?.photoUrl;
  const fallbackSidePhotos = fallbackPhotos.slice(1) || [];
  
  // Use fallback if no real photos available
  const finalMainPhoto = mainPhoto || fallbackMainPhoto;
  const finalSidePhotos = sidePhotos.length > 0 ? sidePhotos : fallbackSidePhotos;
  const finalHasPhotos = realPhotos.length > 0 || fallbackPhotos.length > 0;
  
  // Check if we're still waiting for real images (API returned placeholders)
  const waitingForRealImages = photos && photos.length > 0 && realPhotos.length === 0;
  
  // Debug: Log photo URLs to help identify placeholder issues
  if (photos && photos.length > 0) {
    console.log('🔍 Photo Analysis:', {
      totalPhotos: photos.length,
      allPhotoUrls: photos.map(p => p.photoUrl),
      realPhotosCount: realPhotos.length,
      realPhotoUrls: realPhotos.map(p => p.photoUrl),
      mainPhoto: mainPhoto,
      finalMainPhoto: finalMainPhoto,
      hasPhotos: finalHasPhotos,
      waitingForRealImages: waitingForRealImages
    });
  }

  // Preload real images when they become available
  useEffect(() => {
    if (realPhotos.length > 0) {
      realPhotos.forEach(photo => {
        if (photo.photoUrl && isValidPhotoUrl(photo.photoUrl)) {
          const img = new window.Image();
          img.src = photo.photoUrl;
          console.log('Preloading real image:', photo.photoUrl);
        }
      });
    }
  }, [realPhotos]);

  // ✅ Loading state with skeleton
  if (loading || userLoading || !slug)
    return (
      <Box maxW="7xl" mx="auto" p={6} pt="220px">
        {/* Main image skeleton */}
        <Skeleton height="400px" borderRadius="xl" mb={10} />
        
        {/* Title skeleton */}
        <VStack align="center" spacing={4} mb={10}>
          <Skeleton height="60px" width="300px" />
          <Skeleton height="30px" width="150px" />
          <Skeleton height="40px" width="200px" />
        </VStack>

        {/* Content skeleton */}
        <Flex justify="space-between" align="center" flexDir={{ base: 'column', md: 'row' }}>
          <Box flex="2">
            <VStack align="start" spacing={3} mb={6}>
              <Skeleton height="40px" width="250px" />
              <Skeleton height="40px" width="200px" />
              <Skeleton height="40px" width="180px" />
            </VStack>
            <HStack spacing={4} mt={4}>
              <Skeleton height="50px" width="200px" />
              <Skeleton height="50px" width="120px" />
            </HStack>
          </Box>
          
          <Box flex="3" display="flex" flexDir="column" gap={6}>
            <Skeleton height="200px" />
            <Skeleton height="200px" />
          </Box>
        </Flex>
      </Box>
    );

  if (!model)
    return (
      <Text textAlign="center" mt={10}>
        Model not found.
      </Text>
    );

  // 🔹 Lấy 3 thông số chính
  const accelSpec = specs?.find(
    (s) => s.specName === 'Acceleration 0 - 100 km/h',
  );
  const powerKW = specs?.find((s) => s.specName === 'Power (kW)');
  const powerPS = specs?.find((s) => s.specName === 'Power (PS)');
  const topSpeed = specs?.find((s) => s.specName === 'Top speed');

  const performanceSpecs = [
    accelSpec && {
      head: 'Acceleration 0 - 100 km/h',
      sub: accelSpec.specValue,
    },
    (powerKW || powerPS) && {
      head: 'Power (kW) / Power (PS)',
      sub:
        powerKW && powerPS
          ? `${powerKW.specValue} kW / ${powerPS.specValue} PS`
          : powerKW
          ? `${powerKW.specValue} kW`
          : powerPS
          ? `${powerPS.specValue} PS`
          : 'N/A',
    },
    topSpeed && { head: 'Top speed', sub: topSpeed.specValue },
  ].filter(Boolean);

  // 🔹 Group specs cho TechnicalDrawer
  const groupedSpecs =
    specs?.reduce((acc, spec) => {
      acc[spec.groupName] = acc[spec.groupName] || [];
      acc[spec.groupName].push(spec);
      return acc;
    }, {}) || {};

  // ✅ Check đăng nhập qua context
  const handleContact = () => {
    if (!isAuthenticated) {
      toast.warning('Please sign in first');
      navigate('/auth/sign-in');
      return;
    }
    onOpen();
  };

  return (
    <Box maxW="7xl" mx="auto" p={6} pt="220px" position="relative">
      {/* Loading overlay - shows immediately on page load */}
      {(loading || loadingPhotos || loadingSpecs || !model || waitingForRealImages) && (
        <Box
          position="fixed"
          top="0"
          left="0"
          right="0"
          bottom="0"
          bg="rgba(255, 255, 255, 0.95)"
          backdropFilter="blur(2px)"
          zIndex="1000"
          display="flex"
          alignItems="center"
          justifyContent="center"
          flexDirection="column"
        >
          <Spinner size="xl" color="blue.500" thickness="4px" />
          <Text mt={4} fontSize="lg" fontWeight="500">
            {waitingForRealImages ? 'Waiting for real vehicle images...' :
             !model ? 'Loading vehicle model...' :
             loading ? 'Loading vehicle model...' : 
             loadingPhotos && loadingSpecs ? 'Loading photos and specifications...' :
             loadingPhotos ? 'Loading photos...' :
             loadingSpecs ? 'Loading specifications...' :
             'Loading vehicle details...'}
          </Text>
        </Box>
      )}
      {/* Ảnh chính */}
      {loadingPhotos ? (
        <Skeleton height="400px" borderRadius="xl" mb={10} />
      ) : finalHasPhotos && finalMainPhoto ? (
        <Box position="relative">
          <Image
            src={finalMainPhoto}
            alt={model.name}
            w="100%"
            h="auto"
            borderRadius="xl"
            mb={10}
            loading="eager"
            priority={true}
            onLoad={() => console.log('✅ Main image loaded successfully:', finalMainPhoto)}
            onError={(e) => {
              console.error('❌ Failed to load main image:', finalMainPhoto);
              // Hide the broken image and show fallback
              e.target.style.display = 'none';
              // Try to load the next available image
              const nextPhoto = finalSidePhotos.find(p => p.photoUrl !== finalMainPhoto);
              if (nextPhoto) {
                console.log('🔄 Trying next available image:', nextPhoto.photoUrl);
                e.target.src = nextPhoto.photoUrl;
                e.target.style.display = 'block';
              }
            }}
            fallback={
              <Box height="400px" borderRadius="xl" mb={10} bg="gray.100" display="flex" alignItems="center" justifyContent="center">
                <Text color="gray.500" fontSize="lg">Loading image...</Text>
              </Box>
            }
          />
        </Box>
      ) : waitingForRealImages ? (
        <Skeleton height="400px" borderRadius="xl" mb={10} />
      ) : (
        <Box height="400px" borderRadius="xl" mb={10} bg="gray.100" display="flex" alignItems="center" justifyContent="center">
          <Text color="gray.500" fontSize="lg">No images available</Text>
        </Box>
      )}

      {/* Thông tin xe */}
      <VStack align="center" spacing={1} mb={10}>
        <Text fontSize="6xl" fontWeight="700">
          {model.name}
        </Text>

        {loadingSpecs ? (
          <Box mt={1}>
            <Skeleton height="30px" width="100px" borderRadius="md" />
          </Box>
        ) : (
          specs && (
            <Box mt={1}>
              <Box
                bg="gray.200"
                px={3}
                py={1}
                borderRadius="md"
                display="inline-block"
              >
                <Text color="black" fontSize="sm" fontWeight="500">
                  {specs?.find((s) => s.specName === 'Fuel Type')?.specValue ||
                    'N/A'}
                </Text>
              </Box>
            </Box>
          )
        )}

        <Box>
          <Text fontWeight="500" fontSize="lg" mt={4}>
            from ${model.price?.toLocaleString()}
          </Text>
        </Box>
      </VStack>

      {/* Layout specs + ảnh phụ */}
      <Flex
        justify="space-between"
        align="center"
        flexDir={{ base: 'column', md: 'row' }}
      >
        {/* Performance Specs */}
        <Box flex="2">
          <VStack align="start" spacing={3} mb={6}>
            {loadingSpecs ? (
              <>
                <Skeleton height="40px" width="250px" />
                <Skeleton height="40px" width="200px" />
                <Skeleton height="40px" width="180px" />
              </>
            ) : (
              performanceSpecs.map((spec, idx) => (
                <VSpec key={idx} head={spec.head} sub={spec.sub} />
              ))
            )}
          </VStack>

          {/* Buttons */}
          <HStack spacing={4} mt={4}>
            <Button
              variant="outline"
              colorScheme="blackAlpha"
              borderColor="black"
              borderWidth="2px"
              borderRadius="sm"
              color="black"
              size="lg"
              _hover={{ bg: 'blackAlpha.100' }}
              onClick={() => setTechOpen(true)}
              fontWeight="400"
            >
              View all technical details
            </Button>

            <Button
              bg="black"
              color="white"
              size="lg"
              px={8}
              _hover={{ bg: 'gray.700' }}
              fontWeight="500"
              onClick={handleContact}
            >
              Contact
            </Button>
          </HStack>
        </Box>

        {/* Ảnh phụ */}
        {loadingPhotos ? (
          <Box flex="3" display="flex" flexDir="column" gap={6}>
            <Skeleton height="200px" />
            <Skeleton height="200px" />
          </Box>
        ) : (
          finalHasPhotos && finalSidePhotos.length > 0 && (
            <Box flex="3" display="flex" flexDir="column" gap={6}>
              {finalSidePhotos.map((photo, idx) => (
                <Box key={idx} position="relative">
                  <Image
                    src={photo.photoUrl}
                    alt={`${model.name} view ${idx + 1}`}
                    w="100%"
                    borderRadius="lg"
                    loading="eager"
                    onLoad={() => console.log(`✅ Side image ${idx + 1} loaded:`, photo.photoUrl)}
                    onError={(e) => {
                      console.error(`❌ Failed to load side image ${idx + 1}:`, photo.photoUrl);
                      e.target.style.display = 'none';
                    }}
                    fallback={
                      <Box height="200px" borderRadius="lg" bg="gray.100" display="flex" alignItems="center" justifyContent="center">
                        <Text color="gray.500" fontSize="sm">Loading...</Text>
                      </Box>
                    }
                  />
                </Box>
              ))}
            </Box>
          )
        )}
      </Flex>

      {/* Drawer kỹ thuật */}
      <TechnicalDrawer
        isOpen={isTechOpen}
        onClose={() => setTechOpen(false)}
        groupedSpecs={groupedSpecs}
      />

      {/* Drawer form mua xe */}
      <PurchaseDrawer isOpen={isOpen} onClose={onClose} vehicle={model} />
    </Box>
  );
}
