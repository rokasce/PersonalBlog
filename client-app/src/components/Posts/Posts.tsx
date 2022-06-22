import axios from "axios";
import React, { useEffect, useState } from "react";
import { Author } from "../author/Author";

interface Post {
  id: string;
  title: string;
  content: string;
  date: string;
}

export const Posts = () => {
  const [posts, setPosts] = useState<Post[]>([]);

  useEffect(() => {
    axios.get("https://localhost:7264/Posts").then((response) => {
      setPosts(response.data);
    });
  }, []);

  return (
    <div
      style={{
        maxWidth: 768,
        display: "flex",
        flexDirection: "column",
        margin: "auto",
      }}
    >
      <Author />
      {posts.map((post) => (
        <div key={post.id}>{post.title}</div>
      ))}
    </div>
  );
};
