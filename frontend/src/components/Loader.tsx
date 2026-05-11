interface Props {
  label?: string;
}

export function Loader({ label }: Props) {
  return (
    <div className="empty-state" style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: 12 }}>
      <span className="loader" />
      {label && <span>{label}</span>}
    </div>
  );
}
