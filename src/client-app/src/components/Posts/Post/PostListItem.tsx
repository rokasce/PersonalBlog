import { Post } from "../../../models/Post";
import styles from "./PostListItem.module.css";

interface Props {
  post: Post;
}

export const PostListItem = ({ post }: Props) => {
  return (
    <div className={styles.post_list_item__wrapper}>
      <h3 className={styles.post_list_item__title}>
        <a href={`/posts/${post.id}`}>{post.title}</a>
      </h3>
      <p className={styles.post_list_item__date}>{post.date}</p>
      <p className={styles.post_list_item__caption}>{post.content}</p>
    </div>
  );
};
