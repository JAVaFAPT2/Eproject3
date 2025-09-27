import React, { useEffect, useMemo, useRef, useState, useCallback } from 'react';
import {
  SimpleGrid,
  Box,
  Image,
  Text,
  Button,
  Flex,
  useBreakpointValue,
  Spinner,
} from '@chakra-ui/react';
import BlogService from 'services/BlogService';

export default function AllBlogs({ columns, searchInput }) {
  const [blogs, setBlogs] = useState([]);
  const [visibleBlogs, setVisibleBlogs] = useState([]);
  const [page, setPage] = useState(0);
  const [loading, setLoading] = useState(true); 

  const firstLoad = 12;
  const nextLoad = 9;

  const loadMoreRef = useRef(null);
  const isDesktop = useBreakpointValue({ base: false, md: true });

  useEffect(() => {
    const loadBlogs = async () => {
      setLoading(true);
      try {
        const res = await BlogService.getBlogs("", 0, 50);
        const data = res.content || res;
        setBlogs(data);
        setVisibleBlogs(data.slice(0, firstLoad));
      } catch (err) {
        console.error("Failed to load blogs:", err);
      } finally {
        setLoading(false);
      }
    };
    loadBlogs();
  }, []);

  const filteredBlogs = useMemo(() => {
    if (!searchInput) return blogs;
    return blogs.filter(blog =>
      (blog.title || "").toLowerCase().includes(searchInput.toLowerCase())
    );
  }, [blogs, searchInput]);

  useEffect(() => {
    setPage(0);
    setVisibleBlogs(filteredBlogs.slice(0, firstLoad));
  }, [filteredBlogs]);

  const loadMore = useCallback(() => {
    const start = firstLoad + page * nextLoad;
    const end = start + nextLoad;
    setVisibleBlogs(prev => [...prev, ...filteredBlogs.slice(start, end)]);
    setPage(prev => prev + 1);
  }, [page, filteredBlogs]);

  useEffect(() => {
    if (isDesktop) return;

    const target = loadMoreRef.current;
    if (!target) return;

    const observer = new IntersectionObserver(
      entries => {
        if (entries[0].isIntersecting) {
          loadMore();
        }
      },
      { threshold: 1.0 }
    );

    observer.observe(target);

    return () => {
      if (target) observer.unobserve(target);
    };
  }, [isDesktop, loadMore]);

  if (loading) {
    return (
      <Flex justify="center" align="center" minH="200px">
        <Spinner size="lg" color="brand.500" />
      </Flex>
    );
  }

  if (!filteredBlogs || filteredBlogs.length === 0) {
    return <Text textAlign="center">Không có dữ liệu</Text>;
  }

  return (
    <>
      <SimpleGrid columns={columns} spacing={6}>
        {visibleBlogs.map((blog, idx) => (
          <Box
            key={idx}
            borderWidth="1px"
            borderRadius="lg"
            overflow="hidden"
            p={4}
          >
            <Image src={blog.thumbnail} alt={blog.title} borderRadius="md" mb={3} />
            <Text fontWeight="bold" noOfLines={2}>
              {blog.title}
            </Text>
            <Text fontSize="sm" color="gray.500" noOfLines={2}>
              {blog.summary}
            </Text>
          </Box>
        ))}
      </SimpleGrid>

      {visibleBlogs.length < filteredBlogs.length && (
        <Flex justify="center" mt={6}>
          <Button colorScheme="brand" onClick={loadMore}>
            Xem thêm
          </Button>
        </Flex>
      )}
    </>
  );
}
