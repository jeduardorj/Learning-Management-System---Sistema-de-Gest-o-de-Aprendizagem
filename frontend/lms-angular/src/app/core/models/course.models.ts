export interface Course { id: string; title: string; description: string; isActive: boolean; createdAt: string; updatedAt?: string; }
export interface CourseRequest { title: string; description: string; }
