import React, { useState, useEffect, useRef } from "react";
import {
  Box,
  Button,
  FormControl,
  FormLabel,
  Input,
  SimpleGrid,
  useToast,
  useColorModeValue,
  Avatar,
  Flex,
  Icon,
  Text,
  Divider,
} from "@chakra-ui/react";
import { MdCameraAlt } from "react-icons/md";
import UserService from "services/UserService";

export default function ProfileTab({ user }) {
  const toast = useToast();
  const bgColor = useColorModeValue("white", "navy.800");
  const textColor = useColorModeValue("secondaryGray.900", "white");
  const sectionBg = useColorModeValue("gray.50", "navy.700");

  const [formData, setFormData] = useState({
    avatarUrl: "",
    fullName: "",
    email: "",
    phone: "",
    addressStreet: "",
    addressWard: "",
    addressDistrict: "",
    addressCity: "",
  });

  const fileInputRef = useRef(null);

  useEffect(() => {
    if (user) {
      setFormData({
        avatarUrl: user.avatarUrl || "",
        fullName: user.fullName || "",
        email: user.email || "",
        phone: user.phone || "",
        addressStreet: user.addressStreet || "",
        addressWard: user.addressWard || "",
        addressDistrict: user.addressDistrict || "",
        addressCity: user.addressCity || "",
      });
    }
  }, [user]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async () => {
    try {
      await UserService.updateProfile(formData);
      toast({
        title: "Profile updated successfully",
        status: "success",
        duration: 3000,
        isClosable: true,
        position: "bottom-right",
      });
    } catch (err) {
      console.error(err);
      toast({
        title: "Update failed",
        status: "error",
        duration: 3000,
        isClosable: true,
        position: "bottom-right",
      });
    }
  };

  const handleFileChange = async (e) => {
    const file = e.target.files[0];
    if (!file) return;

    try {
      const { avatarUrl } = await UserService.updateAvatar(file);
      setFormData((prev) => ({ ...prev, avatarUrl }));

      toast({
        title: "Avatar updated successfully",
        status: "success",
        duration: 3000,
        isClosable: true,
        position: "bottom-right",
      });
    } catch (err) {
      toast({
        title: "Avatar update failed",
        status: "error",
        duration: 3000,
        isClosable: true,
        position: "bottom-right",
      });
    }
  };

  const isGoogleAvatar = formData.avatarUrl?.includes("googleusercontent.com");

  const handleAvatarClick = () => {
    if (!isGoogleAvatar && fileInputRef.current) {
      fileInputRef.current.click();
    }
  };

  return (
    <Box bg={bgColor} p={8} borderRadius="16px" shadow="md">
      {/* Avatar + Basic Info */}
      <Flex direction="column" align="center" justify="center" mb={8}>
        <Box position="relative" w="140px" h="140px">
          <Avatar
            _hover={{ cursor: isGoogleAvatar ? "default" : "pointer" }}
            src={formData.avatarUrl}
            size="2xl"
            w="140px"
            h="140px"
            border="4px solid"
            borderColor={useColorModeValue("gray.100", "gray.700")}
          />
          {!isGoogleAvatar && (
            <Flex
              position="absolute"
              top="0"
              left="0"
              w="100%"
              h="100%"
              align="center"
              justify="center"
              bg="blackAlpha.500"
              opacity="0"
              _hover={{ opacity: 1, cursor: "pointer" }}
              borderRadius="full"
              onClick={handleAvatarClick}
            >
              <Icon as={MdCameraAlt} w="40px" h="40px" color="white" />
            </Flex>
          )}
        </Box>
        {!isGoogleAvatar && (
          <Input
            type="file"
            accept="image/*"
            ref={fileInputRef}
            onChange={handleFileChange}
            display="none"
          />
        )}
        <Text mt={4} fontSize="xl" fontWeight="bold" color={textColor}>
          {formData.fullName || "Unnamed User"}
        </Text>
        <Text fontSize="sm" color="gray.500">
          {formData.email}
        </Text>
      </Flex>

      <Divider my={6} />

      {/* Personal Information */}
      <Box p={4} bg={sectionBg} borderRadius="12px" mb={6}>
        <Text fontSize="lg" fontWeight="bold" mb={4}>
          Personal Information
        </Text>
        <SimpleGrid columns={{ base: 1, md: 2 }} spacing={6}>
          <FormControl>
            <FormLabel>Full Name</FormLabel>
            <Input
              color={textColor}
              name="fullName"
              value={formData.fullName}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl>
            <FormLabel>Email</FormLabel>
            <Input
              color={textColor}
              name="email"
              value={formData.email}
              onChange={handleChange}
              disabled
            />
          </FormControl>

          <FormControl>
            <FormLabel>Phone</FormLabel>
            <Input
              color={textColor}
              name="phone"
              value={formData.phone}
              onChange={handleChange}
            />
          </FormControl>

          {user?.roleName === "EMPLOYEE" && (
            <FormControl>
              <FormLabel>Hourly Rate</FormLabel>
              <Input
                color={textColor}
                value={user.hourlyRate || ""}
                isReadOnly
              />
            </FormControl>
          )}
        </SimpleGrid>
      </Box>

      {/* Address Information */}
      <Box p={4} bg={sectionBg} borderRadius="12px">
        <Text fontSize="lg" fontWeight="bold" mb={4}>
          Address
        </Text>
        <SimpleGrid columns={{ base: 1, md: 2 }} spacing={6}>
          <FormControl>
            <FormLabel>Street</FormLabel>
            <Input
              color={textColor}
              name="addressStreet"
              value={formData.addressStreet}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl>
            <FormLabel>Ward</FormLabel>
            <Input
              color={textColor}
              name="addressWard"
              value={formData.addressWard}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl>
            <FormLabel>District</FormLabel>
            <Input
              color={textColor}
              name="addressDistrict"
              value={formData.addressDistrict}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl>
            <FormLabel>City</FormLabel>
            <Input
              color={textColor}
              name="addressCity"
              value={formData.addressCity}
              onChange={handleChange}
            />
          </FormControl>
        </SimpleGrid>
      </Box>

      <Button mt={8} colorScheme="brand" color="white" onClick={handleSubmit}>
        Save Changes
      </Button>
    </Box>
  );
}
