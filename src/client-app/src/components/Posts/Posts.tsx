import axios from "axios";
import React, { useEffect, useState } from "react";
import { Post } from "../../models/Post";
import { PostListItem } from "./Post/PostListItem";
import styles from "./Posts.module.css";

export const Posts = () => {
  const [posts, setPosts] = useState<Post[]>([]);

  useEffect(() => {
    axios.get("https://localhost:7264/Posts").then((response) => {
      setPosts(response.data);
    });
  }, []);

  return (
    <div className={styles.posts}>
      {posts.map((post) => (
        <PostListItem key={post.id} post={post} />
      ))}
    </div>
  );
};
