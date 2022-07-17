import styles from "./Author.module.css";
import authorImage from "./author.jpg";

export const Author = () => {
  return (
    <div className={styles.author}>
      <img className={styles.author__image} src={authorImage} alt="logo" />
      <div className={styles.author__info}>
        <span className={styles.author__name}>
          Personal blog by Rokas Cesnulevicius
        </span>
        <span className={styles.author__description}>
          Full-stack .NET developer
        </span>
      </div>
    </div>
  );
};
