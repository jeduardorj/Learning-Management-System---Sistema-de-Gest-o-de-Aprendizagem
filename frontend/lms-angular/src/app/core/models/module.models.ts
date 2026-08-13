export interface Module { id: string; title: string; order: number; courseId: string; lessons?: Lesson[]; createdAt: string; }
export interface Lesson { id: string; title: string; content: string; order: number; moduleId: string; createdAt: string; }
export interface ModuleRequest { title: string; order: number; }
export interface LessonRequest { title: string; content: string; order: number; }
